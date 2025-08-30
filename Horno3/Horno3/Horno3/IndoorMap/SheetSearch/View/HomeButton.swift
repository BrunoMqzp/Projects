//
//  HomeButton.swift
//  Horno3
//
//  Created by Alumno on 02/05/25.
//

import Foundation
import SwiftUI

struct HomeButtonMap: View {
    @Binding var isExpanded: Bool
    var body: some View {
        ZStack(alignment: .topLeading){
            MapContainerView(isExpanded: $isExpanded)
            ZStack(alignment: .topLeading){
                Button(action: {
                    withAnimation {
                        isExpanded.toggle()
                    }
                })
                {
                    Image(systemName: "house.circle.fill")
                        .resizable()
                        .font(.title)
                        .foregroundColor(.hornoOrange)
                        .background(.white)
                        .clipShape(.circle)
                        .frame(maxWidth: 75, maxHeight: 75)
                        .padding(.leading, 25)
                        .padding(.top, 50)
                }
            }
        }
    }
}
