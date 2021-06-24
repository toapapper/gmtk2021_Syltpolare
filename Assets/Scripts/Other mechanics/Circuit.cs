using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{
    public bool poweredByDefault = false;
    public bool powered = false;

    protected Socket[] sockets;
    protected bool[] socketsRecievingPower;//Sockets som ger kraft till denna kretsen, hittas via index
    

    //kanske ska tillåta att man kan ge kraft till kretsen via sig självt.... skulle va lite roligt. dessutom tar det upp två hela sockets så...

        /*
         * -gör bfs nodsökning varje gång som någon plugs status ändras
         * -lista med circuits som har testats, skickas vidare i sökningen som typ e rekursiv.
         * -kommer inte behöva notifiera alla, bara de som är kopplade till nätverket.
         * -måste kolla igenom alla noder i nätverket antar jag(?), lär inte bli stora nätverk ändå men asså.
         * Om kraft tappas här pga urpluggat så tappas kraft i alla andra sammankopplade om inte nån av de fortfarande har kraft då asså.
         * äh, blir bra och inte så osimpelt
         * Behöver metod: StartSearch och bool Search(ref List<Circuit> searched)
         * om minst en bool returnas true så e hela nätverket av circuits powered
         * 
         * Sök genom att kolla sockets -> plugs -> base -> annan plug->circuit E ju bara så circuits kopplas samman ju.. antar jag. Kan lägga till bool inherentlyPowered i cableBase och circuit
         * Varken eller av de om man möter den bör stoppa sökningen om den inte är en av de som ändrats som startade sökningen. Isåfall fixas det i startsearch.
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

    //letar genom alla circuits som är kopplade med den
    //Den fortsätter förbi alla noder och vägar som inte är "inherentlyPowered"
    //Den fortsätter tills den har hittat alla kretsar, men result blir true så länge minst en kraftkälla hittas ihopkopplad
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
                            if (plugsocketCircuit.poweredByDefault)//om har nått en kraftkälla så är resultatet för hela nätverket sant
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
