/*
See the LICENSE.txt file for this sample’s licensing information.

Abstract:
The decoded representation of an IMDF Opening feature type.
*/

import Foundation

class Opening: Feature<Opening.Properties> {
    struct Properties: Codable {
        let category: String
        let levelId: UUID
    }
    
    var openings: [Opening] = []
}

// For more information about this class, see: 

