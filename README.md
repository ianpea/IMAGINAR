# Information

Contact: ian.peewujian@gmail.com

Done by Pee Wu Jian during January 2020 - Febraury 2020.

# Links
Video Link: https://www.youtube.com/watch?v=6R38CkaivKQ

# IMAGINAR
IMAGINAR (IMAGIN-A-R) is an Augmented Rreality (AR) game in which the player can imagine "things" and transform into a pet that can fight imaginary enemies. This game is built using the MVC design pattern.

# Gameplay
## Imagination & Enemies
The game features randomly spawned "imaginations" such as
* Cute animals (AI),
* Phenomenons,
* Boss,
* Enemies (AI).
that can be collected or attacked. The player must collect imaginations because attacks uses imagination points as mana points to fight enemies with bullets and laser beam.

## Boss
<img src = "/Images/Bossku.jpg" width=650 height=350>

## Form & Transformation
In this game, you must leverage between your *human* and *pet* forms! Current forms & transformation can be seen on the top side on the screen! (Green = Human, Purple = Pet)

### Human

<img src = "/Images/Animal.jpg" width=200 height=400><img src = "/Images/Phenomenon.jpg" width=200 height=400><img src = "/Images/Ally.jpg" width=200 height=400>

As a Human, you can collect imaginations (animals, phenomenon, etc) to boost your score and iamgination points. You can also spawn Allies (Cannibal Flower) with imagination points to help you fight enemies!
### Pet

<img src = "/Images/PetForm.jpg" width=200 height=400>

As a Pet, you can attack enemies with bullets and laser beam! Notice the change of UI to indicate the forms.
# Scoring/Objective
**More imagination points collected = More score!**
The objective of the game is to get the highest score possible! Every action you do in the game will impact your imagination points (collect,kill -> add, attack -> deduct). Everytime you stopped playing the game, the game will save the current score so that you can resume playing on the score in the next session of playing. Also, your score are persistant, but your imagination points are not. So, everytime you start a new session, you will need to obtain some imagination points before you can start attacking!

# Some Code Features
## Object Pooling
Object pooling is used to spawn the various imaginations & enemies randomly.

<img src="/Images/PoolScript.PNG">

## AI

### Ally
This is the AI for ally (Cannibal Flower) which attacks nearby enemies by shooting at them.

<img src="/Images/AllyAI.PNG">

### Enemies
This is the AI for enemies. They will wander or move towards the player depending on the player's current transformation.

<img src="/Images/MonsterAI.PNG">

# UI Manager

<img src="/Images/UIManagerScript.PNG">


