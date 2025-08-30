//
//  Eventos.swift
//  Horno3
//
//  Created by Aleksandra on 01/04/25.
//

import SwiftUI

struct Eventos: View {
    @StateObject private var viewModel = EventosViewModel()
    
    var body: some View {
        VStack() {
            ScrollView(.horizontal, showsIndicators: false) {
                HStack(spacing: 15) {
                    ForEach(viewModel.eventos) { evento in
                        NavigationLink(destination: DetalleEvento(evento: evento)) {
                            VStack {
                                Image(evento.imagen)
                                    .resizable()
                                    .scaledToFill()
                                    .frame(width: 180, height: 150)
                                    .cornerRadius(10)
                                    .shadow(radius: 5)
                                
                                Text(evento.titulo)
                                    .font(.headline)
                                    .foregroundColor(.hornoOrange)
                                
                                Text(evento.descripcion)
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
    Eventos()
}

