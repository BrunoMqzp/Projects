import SwiftUI
import AVFoundation
import Vision
import CoreML
import Translation

struct Camara: UIViewRepresentable {
    let session: AVCaptureSession
    func makeUIView(context: Context) -> UIView {
        let view = UIView(frame: UIScreen.main.bounds)
        let previewLayer = AVCaptureVideoPreviewLayer(session: session)
        previewLayer.videoGravity = .resizeAspectFill
        previewLayer.frame = view.frame
        view.layer.addSublayer(previewLayer)
        return view
    }
    func updateUIView(_ uiView: UIView, context: Context) {}
}

class AudioPlayer: ObservableObject {
    private var audioPlayer: AVAudioPlayer?
    
    func playSound(named soundName: String) {
        guard let url = Bundle.main.url(forResource: soundName, withExtension: "mp3") else { return }
        do {
            audioPlayer = try AVAudioPlayer(contentsOf: url)
            audioPlayer?.play()
        } catch {}
    }
    
    func stopSound() {
        audioPlayer?.stop()
    }
}

struct ObjetoInfo {
    let nombre: String
    let informacion: String
}

enum ScanningMode {
    case imageClassification
    case qrCode
}

class CameraManager: NSObject, ObservableObject, AVCaptureVideoDataOutputSampleBufferDelegate, AVCaptureMetadataOutputObjectsDelegate {
    @Published var scanningMode: ScanningMode = .imageClassification
    @Published var infoForLastDetection: ObjetoInfo?
    @Published var detectedLabels: [String] = []
    @Published var showDetectionText = false
    @Published var detectedURL: URL?
    private var classificationModel: VNCoreMLModel?
    private var videodataOutput: AVCaptureVideoDataOutput?
    private var metadataOutput: AVCaptureMetadataOutput?
    private var lastDetectionTime: Date?
    private var currentDetection: String?
    private var detectionTimer: Timer?
    private let sessionQueue = DispatchQueue(label: "com.museo.cameraQueue")
    let CaptureSession = AVCaptureSession()
    private let datosDeObjeto: [String: ObjetoInfo] = [
        "Horno 1": ObjetoInfo(
            nombre: "Horno 1",
            informacion: "Es una pieza clave del patrimonio industrial de México. Formó parte de la Fundidora de Fierro y Acero de Monterrey, la primera siderúrgica de América Latina, fundada en 1900. Este horno, junto con otros, fue esencial en la producción de acero que impulsó el desarrollo industrial del país durante el siglo XX."
        ),
        "Valvulas de Gas": ObjetoInfo(
            nombre: "Valvulas de Gas",
            informacion: "Las válvulas de gas son componentes esenciales para garantizar un manejo seguro y eficiente de los sistemas de gas. Estas válvulas regulan el flujo, la presión y la dirección del gas en diversas aplicaciones industriales, desde la producción hasta el almacenamiento y transporte.​"
        ),
        "Planeta Niños": ObjetoInfo(
            nombre: "Planeta Niños",
            informacion: "En esta experiencia vivirás el planeta tierra de una manera totalmente diferente a como lo has visto antes, esta experiencia es parte fundamental de nuestro compromiso por divulgar la importancia del cuidado del medio ambiente."
        ),
        "Estrella de Jorge": ObjetoInfo(
            nombre: "Estrella de Jorge",
            informacion: "Escultura que nos da un vision de la conexion que tiene monterrey con el Horno3"
        ),
        "Pantaloneras": ObjetoInfo(
            nombre: "Pantaloneras",
            informacion: "Eran prendas de protección utilizadas por los trabajadores para salvaguardar sus piernas y torso inferior de los riesgos asociados al ambiente industrial, como el contacto con metales fundidos, chispas y calor extremo.​"
        ),
        "Caldera": ObjetoInfo(
            nombre: "Caldera",
            informacion: "Una caldera en la industria es un equipo utilizado para generar vapor o agua caliente para procesos industriales. Las calderas son esenciales en diversas industrias, como la petroquímica, alimentaria, papelera y textil, entre otras. Estos equipos son fundamentales para crear el vapor necesario en procesos de producción, calefacción y otros usos industriales."
        ),
        "Horno Pricipal": ObjetoInfo(
            nombre: "Horno Principal",
            informacion: "En 1968 entró en operaciones el Horno Alto N° 3 que colocó a la Compañía Fundidora como la empresa más antigua con la tecnología más moderna de América Latina, al poseer un Horno Alto de mayor capacidad y automatización. Por dieciocho años el Horno Alto N° 3 iluminó las noches del Monterrey hasta el 9 de mayo de 1986, año en que se apagó para siempre y Fundidora Monterrey cerrara sus puertas definitivamente."
        ),
        "Default": ObjetoInfo(
            nombre: "NADA",
            informacion: "Lo siento, no se encontraron objetos :("
        )
    ]

    override init() {
        super.init()
        setupModel()
        checkPermissions()
        setupCamera()
    }
    
    private func setupModel() {
        do {
            let config = MLModelConfiguration()
            let mlModel = try MuseoImagenesML(configuration: config)
            classificationModel = try VNCoreMLModel(for: mlModel.model)
        } catch {
            print("Error!!!!: \(error.localizedDescription)")
        }
    }

    private func setupCamera() {
        sessionQueue.async { [weak self] in
            guard let self = self else { return }
            guard let device = AVCaptureDevice.default(.builtInWideAngleCamera, for: .video, position: .back),
                  let input = try? AVCaptureDeviceInput(device: device) else { return }
            
            self.CaptureSession.beginConfiguration()
            self.CaptureSession.inputs.forEach { self.CaptureSession.removeInput($0) }
            self.CaptureSession.outputs.forEach { self.CaptureSession.removeOutput($0) }
            
            if self.CaptureSession.canAddInput(input) {
                self.CaptureSession.addInput(input)
            }
            
            self.configureOutputs()
            self.CaptureSession.commitConfiguration()
            
            if !self.CaptureSession.isRunning {
                self.CaptureSession.startRunning()
            }
        }
    }
    
    private func configureOutputs() {
        switch scanningMode {
        case .imageClassification:
            let output = AVCaptureVideoDataOutput()
            output.setSampleBufferDelegate(self, queue: DispatchQueue(label: "videoQueue"))
            if CaptureSession.canAddOutput(output) {
                CaptureSession.addOutput(output)
                videodataOutput = output
            }
            
            if let metadataOutput = metadataOutput {
                CaptureSession.removeOutput(metadataOutput)
                self.metadataOutput = nil
            }
            
        case .qrCode:
            let output = AVCaptureMetadataOutput()
            output.setMetadataObjectsDelegate(self, queue: DispatchQueue(label: "metadataQueue"))
            if CaptureSession.canAddOutput(output) {
                CaptureSession.addOutput(output)
                metadataOutput = output
                output.metadataObjectTypes = [.qr]
            }
        
            if let videodataOutput = videodataOutput {
                CaptureSession.removeOutput(videodataOutput)
                self.videodataOutput = nil
            }
        }
    }
    
    func toggleScanningMode() {
        sessionQueue.async { [weak self] in
            guard let self = self else { return }
            self.CaptureSession.beginConfiguration()
            self.scanningMode = self.scanningMode == .imageClassification ? .qrCode : .imageClassification
            self.configureOutputs()
            self.CaptureSession.commitConfiguration()
            
            DispatchQueue.main.async {
                self.resetDetection()
            }
        }
    }
    
    private func resetDetection() {
        lastDetectionTime = nil
        currentDetection = nil
        detectedLabels.removeAll()
        infoForLastDetection = nil
        showDetectionText = false
        detectedURL = nil
        detectionTimer?.invalidate()
        detectionTimer = nil
    }
    
    func captureOutput(_ output: AVCaptureOutput, didOutput sampleBuffer: CMSampleBuffer, from connection: AVCaptureConnection) {
        guard scanningMode == .imageClassification,
              let pixelBuffer = CMSampleBufferGetImageBuffer(sampleBuffer),
              let model = classificationModel else {
            DispatchQueue.main.async {
                self.resetDetection()
            }
            return
        }
        
        let request = VNCoreMLRequest(model: model) { [weak self] request, error in
            self?.procesarClasificacion(request: request, error: error)
        }
        
        request.imageCropAndScaleOption = .centerCrop
        
        try? VNImageRequestHandler(cvPixelBuffer: pixelBuffer, options: [:]).perform([request])
    }
    
    func metadataOutput(_ output: AVCaptureMetadataOutput, didOutput metadataObjects: [AVMetadataObject], from connection: AVCaptureConnection) {
        guard scanningMode == .qrCode,
              let qrObject = metadataObjects.first as? AVMetadataMachineReadableCodeObject,
              let qrValue = qrObject.stringValue else {
            DispatchQueue.main.async {
                self.resetDetection()
            }
            return
        }
        
        DispatchQueue.main.async {
            self.handleQRDetection(qrValue: qrValue)
        }
    }
    
    private func handleQRDetection(qrValue: String) {
        if currentDetection != qrValue {
            currentDetection = qrValue
            lastDetectionTime = Date()
            showDetectionText = false
            detectedLabels.removeAll()
            detectedURL = nil
            detectionTimer?.invalidate()
            detectionTimer = Timer.scheduledTimer(withTimeInterval: 2.0, repeats: false) { [weak self] _ in
                guard let self = self else { return }
                self.showQRInfo(qrValue: qrValue)
            }
        }
    }
    
    private func showQRInfo(qrValue: String) {
        if let url = URL(string: qrValue), UIApplication.shared.canOpenURL(url) {
            detectedURL = url
            detectedLabels = ["Toque para abrir: \(qrValue)"]
        } else {
            detectedLabels = ["QR: \(qrValue)"]
            detectedURL = nil
        }
        
        infoForLastDetection = ObjetoInfo(nombre: "Código QR", informacion: qrValue)
        showDetectionText = true
    }
    
    private func procesarClasificacion(request: VNRequest, error: Error?) {
        guard let results = request.results as? [VNClassificationObservation],
              let topResult = results.first else {
            DispatchQueue.main.async {
                self.resetDetection()
            }
            return
        }
        
        let confidence = Int(topResult.confidence * 100)
        guard confidence > 60 else {
            DispatchQueue.main.async {
                self.resetDetection()
            }
            return
        }
        
        let etiqueta = "\(topResult.identifier) \(confidence)%"
        let objetoInfo = datosDeObjeto[topResult.identifier] ?? datosDeObjeto["Default"]!
        
        DispatchQueue.main.async {
            self.handleObjectDetection(identifier: topResult.identifier, label: etiqueta, info: objetoInfo)
        }
    }
    
    private func handleObjectDetection(identifier: String, label: String, info: ObjetoInfo) {
        if currentDetection != identifier {
            currentDetection = identifier
            lastDetectionTime = Date()
            showDetectionText = false
            detectedLabels.removeAll()
            detectedURL = nil
            detectionTimer?.invalidate()
            detectionTimer = Timer.scheduledTimer(withTimeInterval: 2.0, repeats: false) { [weak self] _ in
                guard let self = self else { return }
                self.showObjectInfo(label: label, info: info)
            }
        }
    }
    
    private func showObjectInfo(label: String, info: ObjetoInfo) {
        detectedLabels = [label]
        infoForLastDetection = info
        showDetectionText = true
        detectedURL = nil
    }
    
    private func checkPermissions() {
        switch AVCaptureDevice.authorizationStatus(for: .video) {
        case .notDetermined:
            AVCaptureDevice.requestAccess(for: .video) { _ in }
        case .denied, .restricted:
            print("Camera access denied")
        default:
            break
        }
    }
    
    func startSession() {
        sessionQueue.async { [weak self] in
            guard let self = self else { return }
            if !self.CaptureSession.isRunning {
                self.CaptureSession.startRunning()
            }
        }
    }
    
    func stopSession() {
        sessionQueue.async { [weak self] in
            guard let self = self else { return }
            if self.CaptureSession.isRunning {
                self.CaptureSession.stopRunning()
            }
        }
    }
}

struct Escanner: View {
    @StateObject private var cameraManager = CameraManager()
    @State private var mostrarDetalle = false
    @State private var objetoSeleccionado: ObjetoInfo?
    
    var body: some View {
        ZStack {
            Camara(session: cameraManager.CaptureSession)
                .edgesIgnoringSafeArea(.all)
            
            VStack {
                Spacer()
                
                if cameraManager.showDetectionText && !cameraManager.detectedLabels.isEmpty {
                    VStack(spacing: 10) {
                        ForEach(cameraManager.detectedLabels, id: \.self) { etiqueta in
                            if let url = cameraManager.detectedURL {
                                Link(destination: url) {
                                    Text(etiqueta)
                                        .font(.headline)
                                        .foregroundColor(.white)
                                        .padding(.horizontal, 14)
                                        .padding(.vertical, 8)
                                        .background(Color.blue.opacity(0.75))
                                        .cornerRadius(10)
                                }
                            } else {
                                Text(etiqueta)
                                    .font(.headline)
                                    .foregroundColor(.white)
                                    .padding(.horizontal, 14)
                                    .padding(.vertical, 8)
                                    .background(Color.purple.opacity(0.75))
                                    .cornerRadius(10)
                                    .onTapGesture {
                                        if let info = cameraManager.infoForLastDetection {
                                            objetoSeleccionado = info
                                            mostrarDetalle = true
                                        }
                                    }
                            }
                        }
                    }
                    .padding(.bottom, 80)
                    .transition(.opacity)
                } else {
                    Text("Enfoca el objeto durante 2 segundos")
                        .font(.headline)
                        .foregroundColor(.white)
                        .padding(12)
                        .background(Color.gray.opacity(0.7))
                        .cornerRadius(8)
                        .padding(.bottom, 80)
                        .opacity(0.7)
                }
                
                Button(action: {
                    cameraManager.toggleScanningMode()
                }) {
                    Text(cameraManager.scanningMode == .imageClassification ? "Cambiar a QR" : "Cambiar a Clasificación")
                        .font(.headline)
                        .foregroundColor(.white)
                        .padding()
                        .background(Color.blue.opacity(0.8))
                        .cornerRadius(10)
                }
                .padding(.bottom, 20)
            }
        }
        .sheet(isPresented: $mostrarDetalle) {
            if let info = objetoSeleccionado {
                ObjetoDetalleView(info: info)
            }
        }
        .onAppear { cameraManager.startSession() }
        .onDisappear { cameraManager.stopSession() }
    }
}


struct ObjetoDetalleView: View {
    let info: ObjetoInfo
    @Environment(\.dismiss) var dismiss
    @StateObject private var audioPlayer = AudioPlayer()
    
    @State var traduccion = ""

    @State var showTranslation = false
    
    func actualizarTraduccion() {
        switch info.nombre {
        case "Horno 1":
            traduccion = "Es una pieza clave del patrimonio industrial de México. Formó parte de la Fundidora de Fierro y Acero de Monterrey, la primera siderúrgica de América Latina, fundada en 1900. Este horno, junto con otros, fue esencial en la producción de acero que impulsó el desarrollo industrial del país durante el siglo XX."
        case "Planeta Niños":
            traduccion = "En esta experiencia vivirás el planeta tierra de una manera totalmente diferente a como lo has visto antes, esta experiencia es parte fundamental de nuestro compromiso por divulgar la importancia del cuidado del medio ambiente."
        case "Horno Principal":
            traduccion = "En 1968 entró en operaciones el Horno Alto N° 3 que colocó a la Compañía Fundidora como la empresa más antigua con la tecnología más moderna de América Latina, al poseer un Horno Alto de mayor capacidad y automatización. Por dieciocho años el Horno Alto N° 3 iluminó las noches del Monterrey hasta el 9 de mayo de 1986, año en que se apagó para siempre y Fundidora Monterrey cerrara sus puertas definitivamente."
        case "Estrella de Jorge":
            traduccion = "Escultura que nos da un vision de la conexion que tiene monterrey con el Horno3"
        case "Valvulas de Gas":
            traduccion = "Las válvulas de gas son componentes esenciales para garantizar un manejo seguro y eficiente de los sistemas de gas. Estas válvulas regulan el flujo, la presión y la dirección del gas en diversas aplicaciones industriales, desde la producción hasta el almacenamiento y transporte.​"
        default:
            traduccion = "Lo siento, no se encontraron objetos :("
        }
    }

    var body: some View {
        NavigationStack {
            VStack(spacing: 20) {
                Text(info.nombre)
                    .font(.largeTitle)
                    .fontWeight(.bold)
                
                ScrollView {
                    Text(info.informacion)
                        .font(.body)
                        .multilineTextAlignment(.center)
                        .padding()
                }

                if info.nombre == "Horno 1" {
                    Button(action: {
                        audioPlayer.playSound(named: "MuseoAu")
                    }) {
                        HStack {
                            Image(systemName: "speaker.wave.2.fill")
                            Text("Escuchar descripción")
                        }
                        .padding()
                        .background(Color.blue)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                    }
                }

                if info.nombre == "Planeta Niños" {
                    Button(action: {
                        audioPlayer.playSound(named: "Audiopla")
                    }) {
                        HStack {
                            Image(systemName: "speaker.wave.2.fill")
                            Text("Escuchar descripción")
                        }
                        .padding()
                        .background(Color.green)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                    }
                }

                if info.nombre == "Horno Principal" {
                    Button(action: {
                        audioPlayer.playSound(named: "Crisol")
                    }) {
                        HStack {
                            Image(systemName: "speaker.wave.2.fill")
                            Text("Escuchar descripción")
                        }
                        .padding()
                        .background(Color.red)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                    }
                }

                if info.nombre == "Estrella de Jorge" {
                    Button(action: {
                        audioPlayer.playSound(named: "Estrellaj")
                    }) {
                        HStack {
                            Image(systemName: "speaker.wave.2.fill")
                            Text("Escuchar descripción")
                        }
                        .padding()
                        .background(Color.red)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                    }
                }

                if info.nombre == "Valvulas de Gas" {
                    Button(action: {
                        audioPlayer.playSound(named: "Informacio")
                    }) {
                        HStack {
                            Image(systemName: "speaker.wave.2.fill")
                            Text("Escuchar descripción")
                        }
                        .padding()
                        .background(Color.red)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                    }
                }

                Spacer()
                
                // Texto traducido
                if showTranslation {
                    Text(traduccion)
                        .font(.title3)
                        .multilineTextAlignment(.center)
                        .transition(.opacity)
                }
            }
            
            .padding()
            .navigationTitle("Detalles")
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Cerrar") {
                        audioPlayer.stopSound()
                        dismiss()
                    }
                }

                ToolbarItem(placement: .primaryAction) {
                    Button {
                        showTranslation.toggle()
                    } label: {
                        Image(systemName: "translate")
                    }
                }
            }
            .onAppear {
                actualizarTraduccion()
            }
            .onChange(of: info.nombre) { _ in
                actualizarTraduccion()
            }
            .translationPresentation(isPresented: $showTranslation, text: traduccion)
        }

    }
}

struct ScannerCoolView: View {
    @State private var showWelcomeText = false
    
    var body: some View {
        NavigationView {
            ZStack {
                Color.white.edgesIgnoringSafeArea(.all)
                
                VStack {
                    if showWelcomeText {
                        Text("Horno 3 Experience")
                            .font(.largeTitle)
                            .fontWeight(.bold)
                            .foregroundColor(.white)
                            .transition(.opacity.combined(with: .scale))
                            .padding(.bottom, 50)
                            .onAppear {
                                DispatchQueue.main.asyncAfter(deadline: .now() + 2) {
                                    withAnimation {
                                        showWelcomeText = false
                                    }
                                }
                            }
                    }
                    
                    Spacer()
                    
                    Text("Bienvenido al Museo Interactivo")
                        .font(.title)
                        .foregroundColor(.white)
                        .padding()
                    
                    NavigationLink(destination: Escanner()) {
                        Text("Iniciar Escáner")
                            .font(.headline)
                            .foregroundColor(.white)
                            .padding()
                            .background(Color.blue)
                            .cornerRadius(10)
                    }
                    
                    Spacer()
                }
                .padding()
            }
        }
        .onAppear {
            withAnimation(.easeInOut(duration: 1.5)) {
                showWelcomeText = true
            }
        }
    }
}
