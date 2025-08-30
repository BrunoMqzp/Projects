//
//  GeojsonViewModel.swift
//  ImplementacionRouting
//
//  Created by Alumno on 01/05/25.
//

import SwiftUI
import Foundation
import CoreLocation

// A friendlier model for your SwiftUI list
class GeoJSONViewModel: ObservableObject {
    @Published var units: [UnitRoute] = []
    @Published var amenities: [AmenityRoute] = []
    init() { loadGeoJSON() }

    func loadGeoJSON() {
        guard
            let url = Bundle.main.url(forResource: "unit", withExtension: "geojson", subdirectory: "IMDFData"),
            let data = try? Data(contentsOf: url)
        else {
            print("❌ Couldn't find unit.geojson")
            return
        }

        do {
            let raw = try JSONDecoder().decode(GeoJSONFeatureCollection.self, from: data)
            self.units = raw.features.map { feat in
                // pull English name or fallback
                let title = feat.properties.name?["en"] ?? "Unnamed"
                // use display_point if present, else first vertex
                let coords = feat.properties.display_point?.coordinates
                    ?? feat.geometry.coordinates.first!.first!
                let loc = CLLocationCoordinate2D(
                    latitude: coords[1],
                    longitude: coords[0]
                )
                return UnitRoute(id: feat.id, name: title, coordinate: loc)
            }
            self.amenities = raw.features.map { feat in
                // pull English name or fallback
                let title = feat.properties.name?["en"] ?? "Unnamed"
                // use display_point if present, else first vertex
                let coords = feat.properties.display_point?.coordinates
                    ?? feat.geometry.coordinates.first!.first!
                let loc = CLLocationCoordinate2D(
                    latitude: coords[1],
                    longitude: coords[0]
                )
                return AmenityRoute(id: feat.id, name: title, coordinate: loc)
            }
        }
        catch {
            print("❌ Decoding error:", error)
        }
    }
}

