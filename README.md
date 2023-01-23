# Remnants of Peak Galeer

Remnants of Peak Galeer è stato realizzato da Marco Andronaco (O46001282) per il corso di Sviluppo Giochi Digitali.

## Punti realizzati
* Splash Screen (logo UniCT)
* Icona gioco
* Load Game
* Options (Sound/music, resolution)
* Credits Screen
* Classifica ordinata, con rimpiazzo dinamico delle entry
* Tutorial (3 schermate statiche)
* Score (gold)
* Gioco a tempo
* Presenza di nemici/sfida
* Difficoltà crescente
* AI base (nell'overworld e in battaglia)
* PlayerPrefs (impostazioni audio e velocità testo/battaglia)
* Singleton (GameMaster)
* Coroutines (BattleManager, UI, ...)
* Enums (BattleManager, MenuManager, OverworldUIManager, ...)
* Classi statiche (UI, AbilityDB)
* Presenza di ereditarietà (Ability e Item estendono Action. Potion, BottledBlessing e tutti gli altri item ereditano da Item. FireBall, Attack, Defend, Heal e tutte le altre abilità ereditano da Ability.)
* Overriding (I vari tipi di NPC eseguono l'override su actionAfterDialogue per definire il codice da eseguire dopo il dialogo)
* Delegates (GameMaster.onSceneChanged, DialogueManager.actionAfterDialogue)
* Animazioni originali (La dialogue box si alza e si abbassa con due animazioni)
* Soundtrack
* Altri suoni
* Raycast (MouseOverInfo, per visualizzare info sull'abilità al passaggio del mouse)
* User Interface
* Particelle (Fuoco nel menu, torcia nella casa iniziale)

## Note
* Ai fini di riutilizzo del codice, ogni item è implementato come una sorta di "abilità consumabile".
* Il codice è scalabile e funziona con un numero variabile di party members che va da 1 a 4, in base ai contenuti di GameMaster.partyNames.
* Tutte le battle stats sono definite in un costruttore di Stats; questo permette di poterle confrontare e bilanciare facilmente tra loro.
* È possibile assegnare una "voce" ai personaggi tramite il parametro Voice dentro NPC, ma al momento non è assegnata per mancanza di asset.
