INCLUDE ../../Variaveis.ink

{examePresunto == 0: -> dialogo0}
{examePresunto > 0: -> dialogo1}

==dialogo0==
É o presunto velho que sobrou duns outros sanduíches que fiz... Será a única coisa que tem pra eu comer? humpf
~ examePresunto = examePresunto + 1
-> END

==dialogo1==
É o presunto velho que sobrou duns outros sanduíches que fiz.
-> END