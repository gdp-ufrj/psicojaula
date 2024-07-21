INCLUDE ../../Variaveis.ink

{exameJornal2 == 0: -> dialogo0}
{exameJornal2 > 0: -> dialogo1}

==dialogo0==
Está rasgado e não consigo ler, mas a reportagem parece interessante. Vou ficar com isso por enquanto.
~ exameJornal2 = exameJornal2 + 1
-> END

==dialogo1==
Diálogo de exame diferente!
-> END
