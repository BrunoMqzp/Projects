//
//  Coordinator.swift
//  ImplementacionRouting
//
//  Created by Alumno on 02/05/25.
//
import SwiftUI
import MapKit


struct MapContainerView: View {
    @StateObject var viewModel = IndoorMapViewModel()
    @Binding var isExpanded: Bool
    @State private var showSearch = false
    @State private var selectedUnit: UnitRoute? = nil
    @State private var vm = GeoJSONViewModel()
    var body: some View {
        NavigationStack{
            ZStack(alignment: .topLeading){
                ZStack(alignment: .topTrailing) {
                    IndoorMapView(viewModel: viewModel, selectedUnit: $selectedUnit)
                        .edgesIgnoringSafeArea(.all)
                    
                    LevelPicker(viewModel: viewModel)
                        .padding(.top, 60)  // Adjust for safe area
                        .padding(.trailing, 15)
                    
                }
                .sheet(isPresented: $showSearch){
                    UnitSearchView(
                        units: vm.units, selectedUnit: $selectedUnit,
                        startUnit: $viewModel.startUnit,
                        endUnit: $viewModel.endUnit
                    ){
                        viewModel.createRoute()
                        showSearch = false
                    }
                }
                /*                .toolbar{
                 Button{
                 showSearch = true
                 } label:{
                 Image(systemName: "magnifyingglass")
                 .resizable()
                 .frame(width: 30, height: 30)
                 .scaledToFit()
                 .accessibilityLabel("Buscar")
                 .padding(.horizontal, 22)
                 .foregroundColor(.orangeOpacity)
                 }
                 .sheet(isPresented: $showSearch){
                 UnitSearchView(
                 units: vm.units,
                 selectedUnit: $selectedUnit
                 )
                 }
                 }*/
                .toolbar{
                    ToolbarItem(placement: .topBarTrailing){
                        Button{
                            showSearch = true
                        } label:{
                            Image(systemName: "magnifyingglass")
                                .resizable()
                                .frame(width: 30, height: 30)
                                .scaledToFit()
                                .accessibilityLabel("Buscar")
                                .padding(.horizontal, 22)
                                .foregroundColor(.orangeOpacity)
                        }
                    }
                    /*.sheet(isPresented: $showSearch){
                        UnitSearchView(
                            units: vm.units,
                            startUnit: $vm.startUnit,
                            endUnit: $vm.endUnit
                        ){
                            vm.createRoute()
                            showSearch = false
                        }
                    }*/
                }
            }
        }
    }
}
#Preview() {
    @Previewable @State var isExpanded = true
    MapContainerView(isExpanded: $isExpanded)
}
