# Changelog

## [0.7.0] - 2026-10-27

### New

- Window rail level with new level design and collisions.
- New desk model and room props with textures.

### Changes

- Improved baked lighting across the environment.
- Push animations now loop.
- Parts that were missing are now shown.

### Bug Fixes

- Corrected jump and push animations.
- Fixed errors from the environment merge.

## [0.6.1] - 2026-10-22

### New

- Jump animations, including charge jump.
- Possibility to climb down when on top of a climbable object.
- Snapping and rotating when grabbing pushable objects.

### Changes

- Player position when grabbing pushable objects now accounts for non square objects.
- Improved climber part detection system for pushable objects.
- Return to idle animation if left immobile while climbing.
- Superior shoot animation replaced with a subtler one.

### Bug Fixes

- Fixed rotation on y and z axes while climbing.
- Fixed offset from the climbable object.
- Corrected colliders of climbable and pushable objects.
- Gravity calculation did not account for delta time.

## [0.6.0] - 2026-10-20

### New

- Climbable object added to the main scene for testing.
- Wwise audio integration.
- Current part text added to the UI.

### Changes

- New player animations and refactored the animation state machine.
- Friction deceleration added to movement.
- Removed the grabber hand model.

### Bug Fixes

- Shoot animation did not play correctly.
- Jumper part did not work correctly.
- Glider part did not work correctly.
- Airborne acceleration was wrong.
- Fixed an error in the player state machine.
- Fixed bugs in the main scene and added missing colliders.
- Fixed Wwise implementation.

## [0.5.1] - 2026-10-14

### New

- More pushable parts added to the Parts Testing scene.

### Changes

- Adjusted shooter part properties and parent.
- New SkinnedMesh and part models.

### Bug Fixes

- Pushable parts had collider issues when using the mesh.
- Shooter bullets disappeared instantly on the main scene.
- Fixed a bug in the jump bar UI.

## [0.5.0] - 2026-10-13

### New

- Toy Box: parts can be grabbed from it and equipped while searching.
- UI shows only discovered parts.
- Climber part: climbing with vertical movement, entering and exiting teleports.
- Pusher part: pushable blocks with direction depending on approach angle and push direction.
- Shooter part with pooled bullets.
- Glider part.
- Grabber part.
- Arm and leg parts differentiation with model swapping on equip.
- Polished free camera with instructions in the UI.
- New Parts Testing scene.

### Changes

- Pushing rotated objects facilitated.

### Bug Fixes

- Player rotation on movement.
- ScriptableObject bug.
- Free camera parameters.

## [0.4.0] - 2026-10-05

### Changes

- Improved day and night lighting.

### Bug Fixes

- Jump did not change player state.

## [0.3.3] - 2026-10-05

### New

- Free camera with movement and instructions in the UI.

## [0.3.2] - 2026-10-03

### New

- Robot walk animation.
- Animator transitions between player states.

### Changes

- Adjusted camera sensitivity and cinemachine bounds.

### Bug Fixes

- Joints merging bug on local center.

## [0.3.1] - 2026-09-29

### New

- New environment scene with prefabs for main game elements.

### Bug Fixes

- EventBus instantiation made deterministic with a bootstrapper.

## [0.3.0] - 2026-09-29

### New

- Super Jump functionality with charging state and charge UI element.
- Jumper part with restriction based on the part equipped.
- Environment day lighting, posters and decorations.

### Changes

- Player rotation and movement now based on player forward.
- Cinemachine cursor tracking.
- Replaced the robot model.

### Bug Fixes

- Super jump adjusted to avoid mini jumps.

## [0.2.0] - 2026-09-21

### New

- Grab capability with Grabbable interface and GrabHandler.
- Nearby items list for detecting grabbable items.
- Part equipping.
- Basic cinemachine camera movement.
- Models and prefabs added to the Environment scene.

### Bug Fixes

- Camera offset when following the player.
- Fixed models.

## [0.1.0] - 2026-09-15

### New

- Player movement with CharacterController and jump functionality.
- Player state machine with idle, jump and airborne states.
- Blocking objects integration.
- Environment scene with the player controller.
