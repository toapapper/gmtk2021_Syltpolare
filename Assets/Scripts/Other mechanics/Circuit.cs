using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public bool poweredByDefault = false;
    public bool powered = false;

    protected Socket[] sockets;
    protected bool[] socketsRecievingPower;//Sockets som ger kraft till denna kretsen, hittas via index
    

    //kanske ska till�ta att man kan ge kraft till kretsen via sig sj�lvt.... skulle va lite roligt. dessutom tar det upp tv� hela sockets s�...

        /*
         * -g�r bfs nods�kning varje g�ng som n�gon plugs status �ndras
         * -lista med circuits som har testats, skickas vidare i s�kningen som typ e rekursiv.
         * -kommer inte beh�va notifiera alla, bara de som �r kopplade till n�tverket.
         * -m�ste kolla igenom alla noder i n�tverket antar jag(?), l�r inte bli stora n�tverk �nd� men ass�.
         * Om kraft tappas h�r pga urpluggat s� tappas kraft i alla andra sammankopplade om inte n�n av de fortfarande har kraft d� ass�.
         * �h, blir bra och inte s� osimpelt
         * Beh�ver metod: StartSearch och bool Search(ref List<Circuit> searched)
         * om minst en bool returnas true s� e hela n�tverket av circuits powered
         * 
         * S�k genom att kolla sockets -> plugs -> base -> annan plug->circuit E ju bara s� circuits kopplas samman ju.. antar jag. Kan l�gga till bool inherentlyPowered i cableBase och circuit
         * Varken eller av de om man m�ter den b�r stoppa s�kningen om den inte �r en av de som �ndrats som startade s�kningen. Is�fall fixas det i startsearch.
         */

    // Start is called before the first frame update
    void Start()
    {
        //inits
        sockets = GetComponentsInChildren<Socket>();

        socketsRecievingPower = new bool[sockets.Length];
        foreach(Socket socket in sockets)
        {
            socket.SetCircuit(this);
        }

        SetPowered(poweredByDefault);

    }

    protected void UpdatePowerStatus()
    {
        List<Circuit> network = new List<Circuit>(5);

        bool result = Search(ref network);
        
        foreach (Circuit circuit in network)
            circuit.SetPowered(result);
    }

    //letar genom alla circuits som �r kopplade med den
    //Den forts�tter f�rbi alla noder och v�gar som inte �r "inherentlyPowered"
    //Den forts�tter tills den har hittat alla kretsar, men result blir true s� l�nge minst en kraftk�lla hittas ihopkopplad
    public bool Search(ref List<Circuit> searchedNodes)
    {
        bool result = false;
        searchedNodes.Add(this);
        for(int i = 0; i < sockets.Length; i++)
        {
            Plug plug = sockets[i].occupiedBy;
            if (plug != null)
            {
                if (plug.cableBase.inherentlyPowered)
                {
                    result = true;
                    continue;
                }

                Plug[] plugs = plug.cableBase.getPlugs();
                for(int j = 0; j < plugs.Length; j++)
                {
                    Socket plugSocket = plugs[j].socket;
                    if(plugSocket != null)
                    {
                        Circuit plugsocketCircuit = plugSocket.circuit;
                        if(plugsocketCircuit != null)
                        {
                            if (plugsocketCircuit.poweredByDefault)//om har n�tt en kraftk�lla s� �r resultatet f�r hela n�tverket sant
                            {
                                result = true;
                                continue;
                            }
                            else if (!searchedNodes.Contains(plugsocketCircuit))
                            {
                                bool tempResult = plugSocket.circuit.Search(ref searchedNodes);
                                if (!result)
                                    result = tempResult;
                            }
                        }
                        else if (plugSocket.powered)
                        {
                            result = true;
                            continue;
                        }
                    }
                }
            }
        }

        return result;
    }

    protected void SetPowered(bool power)
    {
        powered = power;

        for (int i = 0; i < sockets.Length; i++)
        {
            sockets[i].powered = power;
        }
    }

    public void SocketUnplugged()
    {
        UpdatePowerStatus();
    }

    public void SocketPluggedIn()
    {
        UpdatePowerStatus();
    }

    
}
