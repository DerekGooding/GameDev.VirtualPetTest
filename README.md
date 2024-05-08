# Virtual Pet Console App C#
## Description
[TopScreenImage](TopScreenSS.png)
#### A Virtual Pet console application in c#. Is loosely based on the original Tamagotchi (p1/p2) released in 1998. Take care of your pet's needs as it grows into an adult. Feed, Play and Care to grow a healthy pet. If you neglect it then it will die :(.

## Controls
### Menus
#### 'A' - Confirm Selection
#### 'S' - Cycle Selection
#### 'D' - In main menu (Jump to top of menu/reset screen), In sub menu (Return to main menu)
### Games
#### 'A' - Left/Lower
#### 'S' - Right/Higher
#### 'D' - Exit Game

## Features/Operation
#### Top screen is where pet is displayed/games are played. The three control buttons are displayed under this screen. Bottom screen displays all text/menus/information.
#### Upon starting a new game the player is given an egg. After a time the egg will hatch and player can name their new pet. The name must be 1-10 characters in length.
#### Then the game starts and the player is shown the main menu. Main menu contains the following options; Stats, Food, Shop, Games, Care, Light, Exit. Cycle through the menu options with 'S', confirm selection with 'A', jump back to top of menu/refresh screen with 'D'. If in a submenu press 'D' to return to main menu.
#### Stats screen displays pets current stats, press any key to return to main menu.
#### Food screen shows all owned food, select a food item to feed pet. Food currently available; Rice Bowl, Fried Eggs, Cake, Steak(after purchasing).
#### Feeding fills the hungry meter and increases pet weight by 1. If the food is a 'snack' it will increase the happy meter by 1 and weight by 2. If the pets hungry meter is full (5) you can't feed it.
#### Shop screen shows available items for purchase. Currently only steak and dice are available in the shop. Steak costs $50 and adds steak to the food menu. Dice costs $150 and adds "Higher or Lower?" to the games menu. Items can only bought once and will show "Sold Out" once bought.
#### Games screen shows all owned games. Games currently available; Left or Right?, Higher or Lower?(after purchasing). Playing a game reduces weight by 1 and if player got 2 or more points increases happiness by 1. Get $10 for each point scored. Games have 5 rounds and can be exited early by pressing 'D'.
#### Left or Right? game; Player must guess which way the pet is going to point. 'A' for left, 'S' for right. It's 50/50 which way it goes.
#### Higher or Lower? game; Player must guess if the next number will be higher or lower than the previous number. Pet shows previous number to the left, with the new number shown on the right. 'A' for higher, 'S' for lower. The number is randomly selected from 1-9.
#### Care screen shows care options; Clean, Medicine, Discipline. Clean gets rid of all poops on screen. Medicine cures pet from sickness. Discipline is for when pet wrongly asks for attention.
#### Light will put the pet to sleep when selected. While sleeping cannot do anything except look at stats, turn light back on, exit game.
#### Exit closes the game. This is the only way to save progress on your pet. When the game is closed, pet activity is not tracked. Pet can only progress while game is running.

## Mechanics
### Hunger
#### Your pet has a hunger meter that can go from 0 (starving) to 5 (full). Over time the pets hunger meter will go down. Feed it to increase. Everytime hunger goes down, pet will poop.
### Happiness
#### Your pet has a happiness meter that can go from 0 (miserable) to 5 (overjoyed). Over time the pets happiness meter will go down. Feed snacks or play games to increase.
### Weight
#### Feed pet to increase weight. Starve or play games to reduce weight. Currently weight does nothing but it may be used to influence evolutions in a future update.
### Attention
#### When hungry or unhappy a '!' will appear above your pet. This lets you know that something is wrong. Sometimes the pet will call for attention when it is not hungry or unhappy.
### Discipline
#### Your pet has a discipline meter that can go from 0 (wild animal) to 5 (obedient). If your pet calls for attention unnecessarily, discipline it by selecting 'Discipline' in the 'Care' menu. This will increase the discipline meter by 1. Discipline is used for deciding evolutions.
### Care Level
#### A hidden stat that tracks how well cared for your pet is. Care mistakes lower care level by 1. Care mistakes include; trying to feed when full, letting pet starve, feeding snacks, letting pet be unhappy, exiting game without putting pet to sleep, letting pet get/stay sick, giving medicine unnecessarily, disciplining a valid attention call, leaving poop uncleaned. Care bonuses increase care level by 1. Care bonuses include; putting pet to sleep before exiting, feeding, keeping pet full, getting 2+ score in games, keeping pet happy, curing sickness, correctly disciplining pet, cleaning poop.
### Death Mistakes
#### A hidden stat that starts at 0 and increases as death mistakes are made. If death mistakes reaches a high enough number there is a chance for your pet to die. Death mistakes slowly decrease until back at 0. Death mistakes include; letting pet starve, leaving pet sick.
### Sickness
#### Pet can get sick. Displays a 'X' above the pets head. Can get sick by; letting pet starve, leaving pet sad, not cleaning poop. Cure with medicine.
### Age
#### Pets age goes up by 1 for everyday it's alive.

## Future Features
#### I'm still a beginner so I'd like to revisit this later on and try refactor/improve all the dumb stuff im doing right now. I'll probably be tweaking the timings of everthing.
#### These are things I may add in the future.
#### -More food and game options
#### -More animations for actions
#### -Improve the screen
#### -Have it always running (updates pet based on time passed since last opened)
#### -Variable timers for each evolution (e.g. baby evolves and gets hungry faster)
#### -More pets and evolution trees.
#### -Other stuff...

## Questions
### What was your motivation?
#### I found my old Tamagotchi, thought it would be cool to recreate it in Unity, realised I had no idea how to do it, decided to do it as a console app instead.
### Why did you build this project?
#### To have something similar to the old Tamagotchi that I could play on my pc.
### What problem does it solve?
#### The problem of not having a virtual pet on your pc?
### What did you learn?
#### This is my first time using the timers to make events happen. Still not sure if it would've been better to use something else or have multiple timers for each event instead of lowering counters each time the timer pops. I had to figure out how to save and load the game between sessions. I was originally trying to have it track how much time passed since last use and figure out what the current stats would be based on that, just like how a Tamagotchi would always run. Decided in the end to only track while the program is running. Used json to store data for the first time. Again probably a better way than how I did it. Overall there's lots of things always being changed and updated as the program runs. Making all of it happen without getting messed up was hard and I feel like my code was getting very spaghetti near the end of making this.
### What makes your project stand out?
#### It's a pretty faithful recreation of the original Tamagotchi (p1/2) (See the functions here: https://tamagotchi.fandom.com/wiki/Tamagotchi_(1996_Pet)#Functions ). Can run it on your desktop and doesn't take up too much screen space. Might enjoy it if you liked the old Tamagotchis but don't want to play one of the smartphone apps.
