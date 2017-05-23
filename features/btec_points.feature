Feature: Calculate BTEC points
  BTEC points are calculated from the number of passes, merits, and distinctions
  Passes are worth 70
  Merits are worth 80
  Distinctions are worth 90
  
  Scenario Outline: Calculating points
    Given the application is open
    When I enter <passes> passes
      And <merits> merits
      And <distinctions> distinctions
    Then it should print out that I have <points> points

    Examples:
      | passes | merits | distinctions | points |
      |   18   |   0    |      0       |  1260  |
      |   0    |   18   |      0       |  1440  |
      |   0    |   0    |      18      |  1620  |
