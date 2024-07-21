INCLUDE ../../Variaveis.ink

{exameJornal1 == 0: -> dialogo0}
{exameJornal1 > 0: -> dialogo1}

==dialogo0==
Está rasgado e não consigo ler, mas a reportagem parece interessante. Vou ficar com isso por enquanto.
~ exameJornal1 = exameJornal1 + 1
-> END

==dialogo1==
Diálogo de exame diferente!
-> END
