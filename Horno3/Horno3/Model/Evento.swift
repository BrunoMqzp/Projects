//
//  File.swift
//  Horno3
//
//  Created by Aleksandra on 01/04/25.
//

import Foundation

struct Evento: Identifiable, Codable {
    let id = UUID()
    let imagen: String
    let titulo: String
    let descripcion: String
}
