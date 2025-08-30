//
//  geojsondecoder.swift
//  ImplementacionRouting
//
//  Created by Alumno on 01/05/25.
//
import CoreLocation
import SwiftUI
import Foundation

struct GeoJSONFeatureCollection: Codable {
    let type: String
    let features: [GeoJSONFeature]
}

struct GeoJSONFeature: Codable, Identifiable {
    let id: String
    let properties: Properties
    let geometry: Geometry
}

struct Properties: Codable {
    // make name optional
    let name: [String:String]?
    let display_point: DisplayPoint?
}

struct Geometry: Codable {
    let type: String
    let coordinates: [[[Double]]]
}

struct DisplayPoint: Codable {
    let type: String
    let coordinates: [Double]
}

// A simpler Unit model for the UI
struct UnitRoute: Identifiable {
    let id: String
    let name: String
    let coordinate: CLLocationCoordinate2D
}
extension UnitRoute: Equatable{
    static func == (lhs: UnitRoute, rhs: UnitRoute) -> Bool {
        return lhs.id == rhs.id
    }
}
struct AmenityRoute: Identifiable {
    let id: String
    let name: String
    let coordinate: CLLocationCoordinate2D
}

struct FeatureRoute: Decodable{
    let type: String
    let feature_type: String?
    let id: String
    let properties: Properties
    let geometry: Geometry
}
