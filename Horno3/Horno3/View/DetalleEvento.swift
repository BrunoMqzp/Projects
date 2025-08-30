//
//  EventoDetalle.swift
//  Horno3
//
//  Created by Alumno on 01/04/25.
//

import SwiftUI

struct DetalleEvento: View {
    let evento: Evento
        
    var body: some View {
        VStack(alignment: .leading) {
                Image(evento.imagen)
                    .resizable()
                    .scaledToFit()
                    .frame(height: 250)
                    .cornerRadius(15)
                    .padding()
                
                Text(evento.titulo)
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .foregroundColor(.hornoOrange)
                    .padding()
                
                Text(evento.descripcion)
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
    DetalleEvento(evento: Evento(imagen: "museo", titulo: "The Real Dream", descripcion: "Un museo impresionante con arte renacentista y moderno."))
}
