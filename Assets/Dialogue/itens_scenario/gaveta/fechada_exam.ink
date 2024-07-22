INCLUDE ../../Variaveis.ink

{exameGavetaFechada == 0: -> dialogo0}
{exameGavetaFechada > 0: -> dialogo1}

==dialogo0==
Esta gaveta está trancada? Não me lembro dela ter tranca, e muito menos de eu ter a chave.
~ exameGavetaFechada = exameGavetaFechada + 1
-> END

==dialogo1==
Trancada. Definitivamente trancada.
-> END