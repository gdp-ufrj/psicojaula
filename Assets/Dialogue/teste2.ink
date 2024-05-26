INCLUDE Variaveis.ink

{dialogoTV == 0: -> dialogo0}
{dialogoTV > 0: -> dialogo1}

==dialogo0==
diálogo televisão diálogo televisão  #character: corvo  #state: corvo_normal
vou parar de assistir TV
preciso encontrar a chave
~ dialogoTV = dialogoTV + 1
-> END

==dialogo1==
eu já interagi com a TV uma vez
este é um diálogo diferente!
-> END