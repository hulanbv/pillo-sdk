//
//  pillo_sdk_developmentApp.swift
//  pillo-sdk-development
//
//  Created by Jeffrey Lanters on 01/11/2021.
//

import SwiftUI

@main
struct Application: App {
    var body: some Scene {
        WindowGroup {
            ContentView()
        }
    }
}

struct ContentView: View {
    var body: some View {
        Text("Hello, Pillo SDK!!!")
            .padding()
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
