# Biomagnet-Leap-Motion-Haptic-Engine
A Unity package for biomagnet-based haptic feedback from virtual objects. 
Using biomagnet stimulation through stereo audio channels and a Leap Motion for precise hand tracking.

### Requirements
- [Lodestone WIRE](https://github.com/AxelFougues/Lodestone-biomagnet-tools/wiki/Lodestone-WIRE) or equivalent. Do not use bluetooth audio it introduces too much delay.
- [Leap Motion Controller 2](https://leap2.ultraleap.com/products/leap-motion-controller-2/) with the proper software installed.
- Unity 2022.3+

### Usage

Plug in your Leap Motion and Lodestone device and run the demo. **Lower your volume to 20-30%**, as you use the demo you can increase it slowly to a safe level that feels comfortable and doesn't overheat your inductors.

On the initial blackboard select which fingers should be stimulated and by what channel. Keyboard turning and mouse navigation change the controls so you can navigate with only the keyboard or the mouse respectively. This can free up your sensing hand.
The equalizer is already set the optimal values for a flat frequency response with a Lodestone Wire but can be modified at will.

Use arrow keys or **WASD** to move and the **mouse** to look around. Hold **E** when instructed to focus on an exhibit. Once focused reach out with your hand and play around with it. Any button press will exit the exhibit.

Things are still touchable without focusing with E so feel free to reach around while exploring to interact with objects and open doors. 

Pause, quit or access the menu with **Esc**.

### Project Structure

The entire demo is contained in a Unity 2022.3.27f1 project and is built through the editor.

- **Stimulation Outputs**: this prefab represents a fingertip (or other stimulated area). It is attached to the leap motion virtual "attachment hands" and is responsible for generating an output when in contact with a touchable object. They can be enabled/disabled and assigned to a channel on the go. They are characterized by a collider, audio sources (signal and audio clips), a Signal Generator script and a Stimulation Output Script.

- **Touchables**: Some example prefabs are given. A touchable object is characterized by a Collider, Rigidbody and Touchable script. There is a collection of Touchable scripts for various interactions.

- **Signal Generator**: This is the code responsible for generating audio signals.

#### Level Hierarchy

- **Level**: contains all non-exhibit objects of the level like decor, floors, walls and some touchable objects that don't need to be focused on.
- **PlayerController**: represents the player and contains the movement controller, camera and hand tracking setup.
- **POIs**: Holds all the focusable exhibits.
- **Menu**: holds the pause menu UI and logic.
