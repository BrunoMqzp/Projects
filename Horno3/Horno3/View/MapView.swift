//
//  MapView.swift
//  Horno3
//
//  Created by Aleksandra on 01/04/25.
//

import SwiftUI

struct MapView: View {
    @State private var isExpanded = true
    var body: some View {
        ZStack(alignment: .top) {
            MapaIndoorView(isExpanded: $isExpanded)
                .frame(width: .infinity, height: .infinity)
                .edgesIgnoringSafeArea(.all)
        }
    }
}

#Preview {
    MapView()
}
