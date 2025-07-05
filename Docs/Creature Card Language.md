Creature Card Language

This is for a creature combat card game inspired by Blue Prince. The idea is that you have cards with unknown power, and you have to learn more about the world to learn what your cards do and how to unlock their full potential. The card text is going to be written in another language. I’ll use English here just to help me get a sense of what im doing. But in the end itll be translated into a different language and potentially also different symbols.

On a player’s turn, they can only do 1 of three things:

-   Play a card
-   Move a card
-   Activate a card’s activatable ability

Turns are very quick.

The board has 3 files (or more). Each player can put 1 creature (or more) onto each file. Creatures in the same file fight each other at the end of the battle. Creatures closest to the enemy are in Rank 1 (frontline). Creatures furthest from the enemy are in Rank 8 (or more) (backline).

Creature stats include:

-   Power
-   RPS symbol (Rock-Paper-Scissors)
-   Cost to play

Creatures don’t really die in this game. You amass an army of them and then total their power. Player with the most power in a file wins that file. Player who wins the most files wins the game.

EX: a R4 creature has more power than a R3 creature, so the R4 creature wins.

A creature that beats an enemy creature with RPS symbols reduces that creature’s power. This means a creature that has lower power can still win through RPS.

EX: a R4 creature has more power than a P3 creature, but P beats R, so the P3 creature beats the R4 creature.

But overwhelming power can still win through the RPS.

EX: a R15 creature has more power than a P3 creature. Even though P beats R, the R15 creature beats the P3 creature.

Triggers:

-   WhenPlayed
-   WhenMoved
-   WhenAllyMoved
-   WhenEnemyMoved

Keywords:

-   Played
-   Moved
-   Ally
-   Enemy
-   Rank
-   File

An ability is made up of 3 things:

-   Trigger
-   Condition
-   Action

When the trigger happens, the condition is checked. If the condition is met, then the action happens.

Most of these triggers happen automatically, at least in the videogame version.

There are no passives. Passives have to be phrased as “when X happens, and Y is true, then Z happens” as described above.

There are no replacement effects. Triggers happen AFTER the event that triggered it. This keeps the abilities simple.

The main reason why we need to keep the abilities simple is so they can more easily be figured out by observation. The cards are going to be near unintelligible, especially at the start of the game when the player doesn’t know anything yet. Hopefully, the cards will be easy to read once the player knows what to look for, but until then, the effects need to be understandable. Visual communication is key. And so is simplicity.

# Creatures

Starter creatures:

-   Cardinal: P1
-   Turtle: R1
-   Rabbit: S1

These 3 give you the option to play RPS anytime. The early battles will be against 1 power creatures, so the RPS mechanic is essential here.

Another early encounter will be a single 3 power creature, to show that power also matters.

The 3 starters have no special abilities, and neither will the first few encounters. This is so the player can more easily pick up on the key info: RPS symbol and power.

# RPS Symbol

Hmmm… maybe the RPS symbol will also contain the power. It’ll be all one symbol, rolled into one.

The creature ability symbol has the three sections as mentioned above. The trigger is the outer most layer. The condition is the middle layer. The action is the core. Each ability symbol has 1 action core. An action core can consist of several actions.

There are multiple pathways to the core. Each trigger has an associated condition. If the trigger happens and its condition is true, it does the effect, ignoring the other trigger/condition pairs.

Some ability symbols might not have every piece. If they don’t have a trigger, it defaults to when this creature is played. If it doesn’t have a condition, it defaults to true. If it doesn’t have an effect, it does nothing. Abilities might also have a cost, which is part of the condition. The default condition of true also has a default cost associated with it, depending on the creature.

Some creatures have abilities by default. You can also add/change abilities on creatures you have.
