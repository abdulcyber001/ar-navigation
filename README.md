# AR Navigation: Augmented Reality-Enabled Interactive Visualization and Navigation System

An offline, Android-based Augmented Reality (AR) navigation application designed for the Department of Electrical and Electronics Engineering at the University of Lagos. This system assists students, staff, and visitors in locating offices, laboratories, and key facilities using QR-code based localization and real-time visual guidance.

## 📌 Overview

Navigating complex university departments can be challenging due to multi-level buildings, numerous laboratories, and inadequate static signage. This project solves that problem by overlaying digital navigation cues directly onto the user's real-world view through a smartphone camera. 

The system operates **fully offline** after installation. It uses strategically placed QR-code markers for multi-origin localization, a graph-based pathfinding algorithm based on field-measured distances, and provides real-time visual guidance via a ground-projected directional arrow, distance remaining, ETA, and a minimap.

## ✨ Key Features

- **Multi-Origin QR Localization:** Scan any of the 15 deployed QR markers to instantly determine your current position and reset AR tracking origin.
- **Graph-Based Pathfinding:** Utilizes Dijkstra's algorithm over a navigation graph built from physical, field-measured distances.
- **Real-Time AR Guidance:** A 3D ground arrow dynamically updates its position and rotation to point toward the next waypoint.
- **Distance & ETA:** Real-time calculations of remaining distance and estimated time of arrival based on an average walking speed.
- **Interactive Minimap:** A top-down 2D map displaying the user's current position, selected destination (red marker), and the full calculated route.
- **Scan Feedback:** Immediate on-screen confirmation when a QR marker is successfully detected.
- **Fully Offline:** No internet connection or GPS is required for navigation.

## 🛠️ Tech Stack

- **Engine:** Unity (2022.3 LTS / Unity 6)
- **AR Framework:** AR Foundation, ARCore XR Plugin
- **Programming Language:** C#
- **QR Decoding:** ZXing.Net
- **UI Framework:** Unity UI Canvas & TextMeshPro
- **IDE:** Visual Studio 2022
- **Target Platform:** Android (Tested on Samsung Galaxy S20 FE)

## 📂 Project Structure & Core Modules

The system is built using modular C# scripts handling specific tasks:

- `QRCodeScanner.cs`: Captures camera frames and uses ZXing to decode QR markers.
- `QRLocalizer.cs`: Maintains the mapping of QR IDs to world coordinates and performs multi-origin localization by repositioning the XR Origin.
- `NavigationGraph.cs`: Stores the node/edge graph of the department and computes the shortest path using Dijkstra's algorithm.
- `PathArrowController.cs`: Uses AR Raycasting to place and orient the 3D ground arrow toward the next waypoint.
- `MinimapController.cs`: Renders the top-down map, updating the user's position and path in real-time.
- `NavigationUI.cs` & `ScanFeedback.cs`: Manages the destination selection dropdown, distance/ETA displays, and success feedback panels.

## 🚀 Getting Started

### Prerequisites
- **Unity Hub** (with Unity 2022.3 LTS or Unity 6 installed)
- **Android Build Support** module (installed via Unity Hub)
- **ARCore-supported Android device** (for deployment)

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/abdulcyber001/ar-navigation.git
