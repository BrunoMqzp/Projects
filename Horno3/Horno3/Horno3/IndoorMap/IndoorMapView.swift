//
//  iPRUEBA.swift
//  Dinoseum
//
//  Created by Alumno on 31/03/25.
//  Copyright © 2025 Apple. All rights reserved.
//
import SwiftUI
import MapKit

struct IndoorMapView: UIViewRepresentable {
    @ObservedObject var viewModel: IndoorMapViewModel
    @Binding var selectedUnit: UnitRoute?
    
    func makeUIView(context: Context) -> MKMapView {
        let mapView = MKMapView()
        mapView.delegate = context.coordinator
        mapView.showsUserLocation = true
        mapView.register(PointAnnotationView.self, forAnnotationViewWithReuseIdentifier: "PointAnnotationView")
        mapView.register(LabelAnnotationView.self, forAnnotationViewWithReuseIdentifier: "LabelAnnotationView")
        
        if let mapRect = viewModel.initialMapRect {
            mapView.setVisibleMapRect(mapRect, edgePadding: .init(top: 20, left: 20, bottom: 20, right: 20), animated: false)
        }
        
        return mapView
    }
    
    func updateUIView(_ uiView: MKMapView, context: Context) {
        context.coordinator.updateMap(for: viewModel.selectedLevelOrdinal, mapView: uiView)
        if let unit = selectedUnit {
            let region = MKCoordinateRegion(
                center: unit.coordinate,
                span: MKCoordinateSpan(latitudeDelta: 0.0005, longitudeDelta: 0.0005)
            )
            uiView.setRegion(region, animated: true)
            
            if let ann = uiView.annotations
                .compactMap({ $0 as? UnitAnnotation})
                .first(where: { $0.unit.id == unit.id})
                    {
                        uiView.selectAnnotation(ann, animated: true)
                }
        }
    }
    
    func makeCoordinator() -> Coordinator {
        Coordinator(viewModel: viewModel)
    }
    
    class Coordinator: NSObject, MKMapViewDelegate {
        var viewModel: IndoorMapViewModel
        
        init(viewModel: IndoorMapViewModel) {
            self.viewModel = viewModel
        }
        
        func updateMap(for ordinal: Int, mapView: MKMapView) {
            guard let venue = viewModel.venue else { return }
            
            // Clear previous overlays and annotations
            mapView.removeOverlays(viewModel.currentLevelOverlays)
            mapView.removeAnnotations(viewModel.currentLevelAnnotations)
            viewModel.currentLevelFeatures.removeAll()
            viewModel.currentLevelOverlays.removeAll()
            viewModel.currentLevelAnnotations.removeAll()
            
            // Add new features
            if let levels = venue.levelsByOrdinal[ordinal] {
                for level in levels {
                    viewModel.currentLevelFeatures.append(level)
                    viewModel.currentLevelFeatures += level.units
                    viewModel.currentLevelFeatures += level.openings
                    
                    let occupants = level.units.flatMap { $0.occupants }
                    let amenities = level.units.flatMap { $0.amenities }
                    viewModel.currentLevelAnnotations += occupants
                    viewModel.currentLevelAnnotations += amenities
                }
            }
            
            let overlays = viewModel.currentLevelFeatures.flatMap { $0.geometry }.compactMap { $0 as? MKOverlay }
            viewModel.currentLevelOverlays = overlays
            
            mapView.addOverlays(overlays)
            mapView.addAnnotations(viewModel.currentLevelAnnotations)
            
            //add Route coordinator
            if let old = viewModel.routeOverlay {
                mapView.removeOverlay(old)
            }
            
            if let routeOverlay = viewModel.routeOverlay {
                mapView.addOverlay(routeOverlay)
            }
        }
        
        // MARK: - MKMapViewDelegate
        func mapView(_ mapView: MKMapView, rendererFor overlay: MKOverlay) -> MKOverlayRenderer {
            //ADD ROUTE
            if let routeOverlay = viewModel.routeOverlay, overlay === routeOverlay {
                let renderer = MKPolylineRenderer(polyline: routeOverlay)
                renderer.strokeColor = .blue
                renderer.lineWidth = 4
                renderer.lineDashPattern = [10, 5]
                return renderer
            }
            guard let shape = overlay as? (MKShape & MKGeoJSONObject),
                  let feature = viewModel.currentLevelFeatures.first(where: { $0.geometry.contains { $0 == shape } }) else {
                return MKOverlayRenderer(overlay: overlay)
            }
            
            let renderer: MKOverlayPathRenderer
            switch overlay {
            case is MKMultiPolygon:
                renderer = MKMultiPolygonRenderer(overlay: overlay)
            case is MKPolygon:
                renderer = MKPolygonRenderer(overlay: overlay)
            case is MKMultiPolyline:
                renderer = MKMultiPolylineRenderer(overlay: overlay)
            case is MKPolyline:
                renderer = MKPolylineRenderer(overlay: overlay)
            default:
                return MKOverlayRenderer(overlay: overlay)
            }
            
            feature.configure(overlayRenderer: renderer)
            return renderer
        }
        
        func mapView(_ mapView: MKMapView, viewFor annotation: MKAnnotation) -> MKAnnotationView? {
            guard let feature = annotation as? StylableFeature else { return nil }
            
            let identifier = feature is Occupant ? "LabelAnnotationView" : "PointAnnotationView"
            let view = mapView.dequeueReusableAnnotationView(withIdentifier: identifier, for: annotation)
            feature.configure(annotationView: view)
            return view
        }
        
        func mapView(_ mapView: MKMapView, didUpdate userLocation: MKUserLocation) {
            guard let location = userLocation.location, let venue = viewModel.venue else { return }
            
            let isInside = venue.geometry.contains { geometry in
                guard let overlay = geometry as? MKOverlay else { return false }
                return overlay.boundingMapRect.contains(MKMapPoint(location.coordinate))
            }
            
            if isInside, let floor = location.floor?.level {
                viewModel.selectedLevelOrdinal = floor
            }
        }
    }
}

struct LevelPicker: View {
    @ObservedObject var viewModel: IndoorMapViewModel
    
    var body: some View {
        VStack(spacing: 8) {
            ForEach(viewModel.levels, id: \.properties.ordinal) { level in
                Button(action: {
                    viewModel.selectedLevelOrdinal = level.properties.ordinal
                }) {
                    Text(level.properties.shortName.bestLocalizedValue ?? "\(level.properties.ordinal)")
                        .font(.system(size: 16, weight: .semibold))
                        .frame(width: 44, height: 44)
                        .background(
                            viewModel.selectedLevelOrdinal == level.properties.ordinal ?
                            Color.blue : Color(UIColor.secondarySystemBackground)
                        )
                        .foregroundColor(
                            viewModel.selectedLevelOrdinal == level.properties.ordinal ?
                            .white : .primary
                        )
                        .cornerRadius(8)
                        .shadow(radius: 2)
                }
            }
        }
        .padding(8)
        .background(.ultraThinMaterial)
        .cornerRadius(12)
        .padding(.trailing, 8)
    }
}
