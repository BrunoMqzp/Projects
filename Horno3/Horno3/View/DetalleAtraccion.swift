//
//  DetalleAtraccion.swift
//  Horno3
//
//  Created by Ranferi Márquez Puig on 24/03/25.
//

import SwiftUI

struct DetalleAtraccion: View {
    let atraccion: Atraccion
    
    var body: some View {
        VStack(alignment: .leading) {
            Image(atraccion.imagen)
                .resizable()
                .scaledToFit()
                .frame(height: 250)
                .cornerRadius(15)
                .padding()
            
            Text(atraccion.titulo)
                .font(.largeTitle)
                .fontWeight(.bold)
                .foregroundColor(.hornoOrange)
                .padding()
            
            Text(atraccion.descripcion)
                .font(.body)
                .foregroundColor(.hornoOrange)
                .padding()
            
            Spacer()
        }
        .navigationTitle("Detalle")
        .navigationBarTitleDisplayMode(.inline)
    }
}

#Preview {
    DetalleAtraccion(atraccion: Atraccion(imagen: "museum1", titulo: "The Real Dream", descripcion: "Un museo impresionante con arte renacentista y moderno."))
}

