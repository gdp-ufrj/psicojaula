INCLUDE ../../Variaveis.ink

{exameCama == 0: -> dialogo0}
{exameCama > 0: -> dialogo1}

==dialogo0==
O único abraço de cada dia, que as vezes me deixa com cãibra. Toda manhã é um desafio me levantar.
~ exameCama = exameCama + 1
-> END

==dialogo1==
Não tenho tempo de ficar nem mais um segundo na cama... Mas como eu gostaria... 
Mal posso esperar pra poder dormir de novo.
-> END
