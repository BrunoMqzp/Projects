//
//  Atracciones.swift
//  Horno3
//
//  Created by Ranferi Márquez Puig on 24/03/25.
//

import SwiftUI
import Foundation

struct Atracciones: View {
    @StateObject private var viewModel = AtraccionesViewModel()
    
    var body: some View {
        VStack() {
            ScrollView(.horizontal, showsIndicators: false) {
                HStack(spacing: 15) {
                    ForEach(viewModel.atracciones) { atraccion in
                        NavigationLink(destination: DetalleAtraccion(atraccion: atraccion)) {
                            VStack {
                                Image(atraccion.imagen)
                                    .resizable()
                                    .scaledToFill()
                                    .frame(width: 180, height: 150)
                                    .cornerRadius(10)
                                    .shadow(radius: 5)
                                
                                Text(atraccion.titulo)
                                    .font(.headline)
                                    .foregroundColor(.hornoOrange)
                                
                                Text(atraccion.descripcion)
                                    .font(.subheadline)
                                    .foregroundColor(.gray)
                                    .lineLimit(2)
                                    .frame(width: 200)
                            }
                            .padding()
                            .background(Color.white)
                            .cornerRadius(15)
                            .shadow(radius: 5)
                        }
                    }
                }
                .padding(.horizontal)
                .padding(.vertical, 8)
            }
        }
    }
}

#Preview {
    Atracciones()
}
