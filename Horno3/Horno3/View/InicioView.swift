import SwiftUI
import MapKit

struct InicioView: View {
    @State private var isExpanded = false
    
    var body: some View {
        ZStack {
                VStack {
                    if isExpanded {
                        ZStack(alignment: .top) {
                            HomeButtonMap(isExpanded: $isExpanded)
                                .frame(width: .infinity, height: .infinity)
                                .edgesIgnoringSafeArea(.all)
                        }
                    } else {
                        ScrollView {
                            VStack(alignment: .leading) {
                                HStack {
                                    
                                    Image("appIconSmall")
                                        .resizable()
                                        .frame(width: 50, height: 50)
                                        .clipShape(Circle())
                                        .padding()
                                    
                                    Text("Bienvenid@ en Horno")
                                        .font(.headline)
                                        .foregroundColor(.hornoOrange)
                                    
                                    + Text("3")
                                        .font(.system(size: 15.0))
                                        .baselineOffset(6.0)
                                }
                                Atracciones()
                                .tabViewStyle(.page)
                                .indexViewStyle(.page(backgroundDisplayMode: .automatic))
                                .frame(height: 200) // Adjust height as needed
                                .padding(.vertical)
                                
                                
                                Eventos()
                                .tabViewStyle(.page)
                                .indexViewStyle(.page(backgroundDisplayMode: .automatic))
                                .frame(height: 220) // Adjust height as needed
                                .padding(.vertical)
  
                            MiniMapView(isExpanded: $isExpanded)
                                    .padding(.horizontal)
                        }
                        .frame(maxWidth: .infinity, maxHeight: .infinity, alignment: .top)
                        .padding(.top, 30)
                        .edgesIgnoringSafeArea(.all)
                    }
                }
            }
        }
        
        .frame(minWidth: 0, maxWidth: .infinity, minHeight: 0, maxHeight: .infinity)
        .ignoresSafeArea(.all)
    }
}

#Preview {
    InicioView()
}
