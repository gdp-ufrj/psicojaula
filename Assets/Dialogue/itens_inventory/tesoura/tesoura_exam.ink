INCLUDE ../../Variaveis.ink

{exameTesoura == 0: -> dialogo0}
{exameTesoura > 0: -> dialogo1}

==dialogo0==
A ponta está bem afiada, deve dar pra cortar qualquer tipo de papel até papelão.
~ exameTesoura = exameTesoura + 1
-> END

==dialogo1==
Com uma ponta afiada dessas, daria até para... Uhhhh, não quero nem pensar nisso!
-> END