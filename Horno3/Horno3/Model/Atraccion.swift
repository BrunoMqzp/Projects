//
//  Atraccion.swift
//  Horno3
//
//  Created by Ranferi Márquez Puig on 24/03/25.
//

import Foundation

struct Atraccion: Identifiable, Codable {
    let id = UUID()
    let imagen: String
    let titulo: String
    let descripcion: String
}
