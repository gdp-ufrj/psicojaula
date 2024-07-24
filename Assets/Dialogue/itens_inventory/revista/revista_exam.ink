INCLUDE ../../Variaveis.ink

{exameRevista == 0: -> dialogo0}
{exameRevista > 0: -> dialogo1}

==dialogo0==
Será que tem alguma coisa atrás do papel?
~ exameRevista = exameRevista+ 1
-> END

==dialogo1==
Humm... se não me engano, colocar um papel contra uma fonte de luz ajuda a revelar imagens escondidas.
-> END