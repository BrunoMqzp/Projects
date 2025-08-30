//
//  MiniMapView.swift
//  MAPImplementacion
//
//  Created by Alumno on 13/03/25.
//

import SwiftUI
import MapKit

struct MiniMapView: View {
    @Binding var isExpanded: Bool
    
    @State private var region = MapCameraPosition.region(
        MKCoordinateRegion(
            center: CLLocationCoordinate2D(latitude: 25.676189952629407, longitude: -100.2825522939145),
            span: MKCoordinateSpan(latitudeDelta: 0.0005, longitudeDelta: 0.0003)
        )
    )
    
    var body: some View {
            Map(position: $region, interactionModes: [])
                .clipShape(RoundedRectangle(cornerRadius: 16))
                .frame(height: 200)
                .onTapGesture {
                    withAnimation {
                        isExpanded.toggle()
                    }
                }
    }
}

/*#Preview {
    MiniMapView(isExpanded: false)
}*/

#Preview() {
    @Previewable @State var isExpanded = false
    return MiniMapView(isExpanded: $isExpanded)
}
