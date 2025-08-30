//
//  SettingsView.swift
//  Horno3
//
//  Created by Ranferi Márquez Puig on 18/03/25.
//

import SwiftUI

struct SettingsView: View {
    @AppStorage("brightness") private var brightness: Double = 0.5
    @AppStorage("language") private var language: String = "Español"
    @AppStorage("darkMode") private var darkMode: Bool = false
    @AppStorage("textSize") private var textSize: Double = 16
    
    let languages = ["Español", "Inglés", "Chino", "Francés", "Portugués"]
    
    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("Pantalla")) {
                    Toggle("Modo Oscuro", isOn: $darkMode)
                        .onChange(of: darkMode) { _ in
                            UIApplication.shared.windows.first?.overrideUserInterfaceStyle = darkMode ? .dark : .light
                        }
                }
                
                Section(header: Text("Ajuste de brillo")) {
                    Slider(value: $brightness, in: 0...1, step: 0.1) {
                        Text("Brillo")
                    }
                    .onChange(of: brightness) { newValue in
                        UIScreen.main.brightness = CGFloat(newValue)
                    }
                }
                
                Section(header: Text("Idioma")) {
                    Picker("Seleccionar idioma", selection: $language) {
                        ForEach(languages, id: \ .self) { lang in
                            Text(lang)
                        }
                    }
                    .pickerStyle(MenuPickerStyle())
                }
                
                Section(header: Text("Tamaño del texto")) {
                    Slider(value: $textSize, in: 12...24, step: 1) {
                        Text("Tamaño de texto")
                    }
                    Text("Vista previa del texto")
                        .font(.system(size: CGFloat(textSize)))
                }
            }
            .navigationTitle("Configuraciones")
        }
    }

}
#Preview {
    SettingsView()
}
