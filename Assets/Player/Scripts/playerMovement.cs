using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;  //Uso il character controller per il player, evitiamo problemi di compenetrazione e non usiamo la fisica

    public float speed = 12f;           //Imposto una velcoità
    public float gravity = -9.81f;      //Imposto la Gravità
    public float jumpHeight = 3f;       //Imposto l'altezza del salto, superficiale per questo progetto

    public Transform groundCheck;       //Imposto un Transform per controllare se ho toccato il terreno
    public float groundDistance = 0.4f; //Imposto una distanza minima per il salto e per fermare il player dalla caduta
    public LayerMask groundmask;        //Imposto un layer per il terreno
        
    Vector3 velocity;                   //imposto un Vector3 per la velocità del player
    bool isGrounded;                    //imposto un valore che mi definisce se ho toccato o no terra



    // Update is called once per frame
    void Update()
    {
        //Se Ha toccato il terreno controllando la distanza allora lo imposto come vero
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        //Di conseguenza se ha toccato e la velocita è sotto zero perchè sta andadndo verso il basso
        if (isGrounded && velocity.y < 0)
        {
            //imposto la velocita y a -2
            velocity.y = -2f;
        }

        //Faccio muovere il player prendendo i valori automatici di unitry di vertical e horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //sommo al valore dello spostamento del player il salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //mantengo la gravita attiva
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
