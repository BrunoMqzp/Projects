//
//  MapaSheetView.swift
//  Horno3
//
//  Created by Alumno on 20/03/25.
//

import SwiftUI

struct MapaSheetView: View {
    
    @State private var selectedDetent: PresentationDetent = .height(100)
    
    var body: some View {
        VStack {


        }
        .frame(maxWidth: .infinity)
        .padding()
        .background(Color.white)
        .cornerRadius(20)
        .shadow(radius: 10)
        .sheet(isPresented: .constant(true)) {
            VStack{
                if (selectedDetent != .height(100)) {
                    Text("We can’t wait to see what you will Create with Swift!")
                }
                
            }
            .presentationDetents([.height(100), .fraction(0.4), .fraction(0.7)], selection: $selectedDetent)
            .presentationDragIndicator(.hidden)
            .interactiveDismissDisabled()
            .presentationBackgroundInteraction(.enabled)
        }
    }
    
}

#Preview {
    MapaSheetView()
}
