- **ENCOUNTER** begins (`Encounter` )
  - Intakes current **PLAYER DATA** and **OPPONENT DATA**
    - ^^Both of type `RuntimeCombatantData`
- 

## `Encounter`

### Notes

The encounter is a standalone scene, the `Encounter` is basically the scene controller and is the guy in charge for the duration of combat.

When the encounter is complete, `Encounter` updates the persistent game state as needed, and fires an **event channel** notifying some DDOL game-manager singleton that combat is complete. The event channel decouples the `Encounter` from whatever DDOL service is managing the game -- no reference required. If the singleton isn't loaded (i.e. standalone mode in the editor), then nothing hears the event, but no errors are raised.

### Parent Class
`GameObject`

### Implements Interfaces
_None_

### Public Properties
_None_

### Editor-Serialized Private properties
- `_encounterFinishedEvent` event channel (`SO_EventEncounterOutcomeDataPayload`) 

### Public Methods
_None_

### Components
...

====

## `RuntimeCombatantData`

### Parent Class
_None_

### Interfaces
_None_

### Public Properties
- `bool IsDead`
- `RuntimeDiceBag DiceBag` 


### Public Methods

### Events

### Components

## `EncounterOutcomeData`

### Public Properties
- `int PlayerXPGained`
- `RuntimeCombatantData NewPlayerState`
  - 


## Singletons

