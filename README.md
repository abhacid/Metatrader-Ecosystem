# Metatrader-Ecosystem
Tools, Robots, Indicators and library for metatrader

# Description
Ce projet regroupe des indicateurs, des robots en Csharp et des outils pour la plateforme Metatrader 4. 
Il fonctionne avec la librairie NQuotes.

# Avantages
Un avantage indéniable du système NQuotes est que l'on peut écrire les indicateurs et robots de trading en CSharp et les éxécuter directemet
sur la plateforme MetaTrader 4, on peut également faire un débogage ou piloter le robot depuis une interface Windows Form ou WPF. 


#installation

Pour compiler le programme avec Visual Studio, il faut au préalable :

1) installer Metatrader 4
2) installer NQuotes
3) créer deux variables d'environnement

1) installer [metatrader 4](http://www.metatrader4.com/).

2) installer [NQuotes](http://www.nquotes.net/installation)  

3) Modifier les instructions post-buids des projets 
  Ces instructions utilisent deux variables d'environnement qu'il faut au préalable définir avec votre {user name} 
  et votre identifiant d'instance de Metatrader qui peut être retrouvée dans le répertoire '%TERMINAL_DATA_PATH%':

  a) TERMINAL_DATA_PATH = C:\Users\{user name}\AppData\Roaming\MetaQuotes\Terminal\
  b) Metatrader_Instance_Id = {identifiant d'instance de Metatrader}
 
# Débogage avec NQuotes

  Pour déboguer le programme avec NQuotes suivez les [instructions](http://www.nquotes.net/expert-creation-tutorial) donnez par son 
  auteur [Daniel](support2@nquotes.net ). 

# Notes
  
  a) si metatrader est actif, il y a une erreur lors de la compilation (la copie des dll ne se fait pas) du fait que les dll sont bloquées en accès par metratrader.
  il faut donc fermer metatrader avant de compiler.

  
# Auteur
Je suis Abdallah Hacid, mon métier est [technicien informatique](http://www.dpaninfor.ovh) et j'habite dans l'Essonne en France.
