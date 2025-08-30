//
//  FullMapView.swift
//  MAPImplementacion
//
//  Created by Alumno on 13/03/25.
//

import SwiftUI
import MapKit

struct FullMapView: View {
    @Binding var isExpanded: Bool
    @State private var selectedDetent: PresentationDetent = .height(100)
    @State private var showSettings = false

    
    @State private var region = MapCameraPosition.region(
        MKCoordinateRegion(
            center: CLLocationCoordinate2D(latitude: 25.676189952629407, longitude: -100.28253083624237),
            span: MKCoordinateSpan(latitudeDelta: 1, longitudeDelta: 1)
        )
    )
    
    var body: some View {
        ZStack{
            Map(position: $region)
                .edgesIgnoringSafeArea(.all)
            ZStack(alignment: .top) {
                Button(action: {
                    withAnimation {
                        isExpanded.toggle()
                    }
                })
                {
                    Image(systemName: "house.circle.fill")
                        .resizable()
                        .font(.title)
                        .foregroundColor(.hornoOrange)
                        .background(.white)
                        .clipShape(.circle)
                        .frame(maxWidth: 75, maxHeight: 75)
                        .padding(.top, 40)
                        .padding(.leading)
                }
                ZStack{
                    //MapaSheetView()
                }
                .sheet(isPresented: .constant(true)) {
                    VStack{
                        //MapaSheetView()
                        if (selectedDetent != .height(100)) {
                            Text("We can’t wait to see what you will Create with Swift!")
                                .font(.title)
                                .padding()
                            
                        }
                        
                    }
                    .presentationDetents([.height(100), .fraction(0.4), .fraction(0.7)], selection: $selectedDetent)
                    .presentationDragIndicator(.visible)
                    .interactiveDismissDisabled()
                    .presentationBackgroundInteraction(.enabled)
                }
            }
            .frame(maxWidth: .infinity, maxHeight: .infinity, alignment: .topLeading)
            .edgesIgnoringSafeArea(.all)
            ZStack(){
                VStack {
                    //Spacer()
                    //MapaSheetView()
                    //  .frame(height: 300)
                    // .transition(.move(edge: .bottom))

                }
            }
        }
    }
}
#Preview() {
    @Previewable @State var isExpanded = true
    return FullMapView(isExpanded: $isExpanded)
}
