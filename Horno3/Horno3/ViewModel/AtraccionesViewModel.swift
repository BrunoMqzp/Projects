//
//  AtraccionesViewModel.swift
//  Horno3
//
//  Created by Ranferi Márquez Puig on 24/03/25.
//

import Foundation

class AtraccionesViewModel: ObservableObject {
    @Published var atracciones: [Atraccion] = []
    
    init() {
        cargarDatos()
    }
    
    func cargarDatos() {
        if let url = Bundle.main.url(forResource: "atracciones", withExtension: "json"),
           let data = try? Data(contentsOf: url) {
            let decoder = JSONDecoder()
            if let atraccionesCargadas = try? decoder.decode([Atraccion].self, from: data) {
                DispatchQueue.main.async {
                    self.atracciones = atraccionesCargadas
                }
            }
        }
    }
}
