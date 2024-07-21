INCLUDE ../../Variaveis.ink

{exameRemedio == 0: -> dialogo0}
{exameRemedio > 0: -> dialogo1}

==dialogo0==
Tomar esses comprimidos deve ser a coisa mais saudável que eu engulo no dia-a-dia. Felizmente sempre tenho um potinho de sobra... 
Embora não me lembro exatamente pra que é esse remédio, sei que sem eles as coisas seriam ainda piores.
~ exameRemedio = exameRemedio + 1
-> END

==dialogo1==
Não devo ter uma overdose se eu tomar mais... ainda assim melhor não gastar mais do que eu preciso.
-> END
