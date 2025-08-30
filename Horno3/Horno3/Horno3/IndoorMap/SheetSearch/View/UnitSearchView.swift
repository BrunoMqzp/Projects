//
//  UnitSearchView.swift
//  ImplementacionRouting
//
//  Created by Alumno on 02/05/25.
//

import SwiftUI
import Foundation

struct UnitSearchView: View {
    @StateObject var viewModel = IndoorMapViewModel()
    let units: [UnitRoute]
    @Binding var selectedUnit: UnitRoute?
    @Environment(\.dismiss) private var dismiss
    
    @State private var searchText = ""
    
    @Binding var startUnit: UnitRoute?
    @Binding var endUnit: UnitRoute?
    let onComplete: () -> Void
    @State private var selectingStart = true
    /*
    var filtered: [UnitRoute]{
        let query = searchText
            .trimmingCharacters(in: .whitespacesAndNewlines)
            .lowercased()
        guard !query.isEmpty else {
            return units
        }
        return units.filter {
            $0.name.lowercased().contains(query)
        }
    }
    
    var body: some View {
        NavigationStack {
            List(filtered){unit in
                Button(action:{
                    selectedUnit = unit
                    
                    dismiss()
                }) {
                    Text(unit.name)
                }
                }
            .searchable(text: $searchText, prompt: "Search Unit")
            .disableAutocorrection(true)
            .navigationTitle("Units")
        }
    }*/
    var body: some View {
        VStack {
            Text(selectingStart ? "Donde estas?" : "A donde vas?")
            List(units){unit in
                Button(unit.name){
                    if selectingStart {
                        startUnit = unit
                        selectingStart = false
                    } else {
                        endUnit = unit
                        onComplete()
                    }
                }
            }
        }
    }
}
