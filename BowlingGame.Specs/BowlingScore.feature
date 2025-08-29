Feature: Bowling score tracking with strikes and spares
  As a player
  I want to keep track of scores for a bowling game with up to 10 players, including strikes and spares
  So that we can determine the correct winner

  Scenario: Player rolls a strike
    Given a new bowling game
      And a player named "Eve"
    When Eve records a score of 10 for frame 1
      And Eve records a score of 3 for frame 2
      And Eve records a score of 4 for frame 2
    Then Eve's total score should be 24

  Scenario: Player rolls a spare
    Given a new bowling game
      And a player named "Frank"
    When Frank records a score of 7 for frame 1
      And Frank records a score of 3 for frame 1
      And Frank records a score of 4 for frame 2
    Then Frank's total score should be 18

  Scenario: Player scores across 10 frames
    Given a new bowling game
      And a player named "Grace"
    When Grace records the following scores:
      | Frame | Roll1 | Roll2 |
      | 1     | 10    | 0     |
      | 2     | 7     | 3     |
      | 3     | 9     | 0     |
      | 4     | 10    | 0     |
      | 5     | 0     | 8     |
      | 6     | 8     | 2     |
      | 7     | 0     | 6     |
      | 8     | 10    | 0     |
      | 9     | 10    | 0     |
      | 10    | 8     | 1     |
    Then Grace's total score should be 146