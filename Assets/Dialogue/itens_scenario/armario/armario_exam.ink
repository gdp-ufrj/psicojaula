INCLUDE ../../Variaveis.ink

{exameArmario == 0: -> dialogo0}
{exameArmario > 0: -> dialogo1}

==dialogo0==
Graças a deus, tem uma blusa limpa, e a jeans não tá muito usada. Salvo por mais um dia!
~ exameArmario = exameArmario + 1
-> END

==dialogo1==
Se eu ficar olhando pra esse armário semi-vazio, com roupa usada e dessarumada, acho que vai bater uma depressão.
-> END