import MapKit

class UnitAnnotation: NSObject, MKAnnotation{
    let unit: UnitRoute
    var coordinate: CLLocationCoordinate2D {unit.coordinate}
    var title: String? {unit.name}
    
    init(unit: UnitRoute) {
        self.unit = unit
        super.init()
    }
}
