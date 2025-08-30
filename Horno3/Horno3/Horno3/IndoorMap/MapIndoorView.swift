//
//  joprueba.swift
//  Dinoseum
//
//  Created by Alumno on 31/03/25.
//  Copyright © 2025 Apple. All rights reserved.
//

import SwiftUI

struct MapaIndoorView: View {
    @StateObject var viewModel = IndoorMapViewModel()
    @Binding var isExpanded: Bool
    @State private var showSearch = false
    @State private var selectedUnit: UnitRoute? = nil
    @State private var vm = GeoJSONViewModel()
    var body: some View {
        ZStack(alignment: .topLeading){
            ZStack(alignment: .topTrailing) {
                MapContainerView(isExpanded: $isExpanded)
            }
        }
    }
}

//#Preview {
  //  MapaIndoorView()
//}

#Preview() {
    @Previewable @State var isExpanded = true
    return MapaIndoorView(isExpanded: $isExpanded)
 }
