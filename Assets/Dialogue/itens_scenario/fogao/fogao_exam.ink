INCLUDE ../../Variaveis.ink

{exameFogao == 0: -> dialogo0}
{exameFogao > 0: -> dialogo1}

==dialogo0==
Será que já paguei o gás esse mês? Sem um fogão o pouco que como vai ser reduzido a nada..
~ exameFogao = exameFogao + 1
-> END

==dialogo1==
O único lugar da casa de onde emana alguma forma de calor, o resto parece sempre tão frio e distante. Eu nem uso muito o forno.
-> END