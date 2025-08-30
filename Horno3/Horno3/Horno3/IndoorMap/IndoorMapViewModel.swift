//
//  ORUEBA.swift
//  Dinoseum
//
//  Created by Alumno on 31/03/25.
//  Copyright © 2025 Apple. All rights reserved.
//

import SwiftUI
import MapKit
import CoreLocation

class IndoorMapViewModel: NSObject, ObservableObject, CLLocationManagerDelegate {
    @Published var venue: Venue?
    @Published var levels: [Level] = []
    @Published var selectedLevelOrdinal: Int = 0
    @Published var initialMapRect: MKMapRect?
    @Published var selectedUnit: Unit?
    @Published var startUnit: UnitRoute?
    @Published var endUnit: UnitRoute?
    @Published var routeOverlay: MKPolyline?
    
    var currentLevelFeatures = [StylableFeature]()
    var currentLevelOverlays = [MKOverlay]()
    var currentLevelAnnotations = [MKAnnotation]()
    private let locationManager = CLLocationManager()
    
    override init() {
        super.init()
        loadIMDFData()
        setupLocationManager()
    }
    
    private func setupLocationManager() {
        locationManager.delegate = self
        locationManager.requestWhenInUseAuthorization()
    }
    
    private func loadIMDFData() {
        let imdfDirectory = Bundle.main.resourceURL!.appendingPathComponent("IMDFData")
        do {
            let imdfDecoder = IMDFDecoder()
            venue = try imdfDecoder.decode(imdfDirectory)
            processLevels()
            setInitialMapRegion()
        } catch {
            print("Error decoding IMDF: \(error)")
        }
    }
    
    private func processLevels() {
        guard let venue = venue else { return }
        
        // Directly access the non-optional dictionary
        let levelsByOrdinal = venue.levelsByOrdinal
        
        let levels = levelsByOrdinal.mapValues { levels in
            levels.first { !$0.properties.outdoor } ?? levels.first!
        }.compactMap { $0.value }
        
        self.levels = levels.sorted { $0.properties.ordinal > $1.properties.ordinal }
        
        if let baseLevel = levels.first(where: { $0.properties.ordinal == 0 }) {
            selectedLevelOrdinal = baseLevel.properties.ordinal
        }
    }
    
    private func setInitialMapRegion() {
        guard let venue = venue, let venueOverlay = venue.geometry.first as? MKOverlay else { return }
        initialMapRect = venueOverlay.boundingMapRect
    }
    
    // Handle location authorization changes
    func locationManagerDidChangeAuthorization(_ manager: CLLocationManager) {
        if manager.authorizationStatus == .authorizedWhenInUse {
            manager.startUpdatingLocation()
        }
    }
    
    func createRoute(){
        guard let start = startUnit, let end = endUnit else { return }
        let coords = [start.coordinate, end.coordinate]
        routeOverlay = MKPolyline(coordinates: coords, count: coords.count)
    }
}
