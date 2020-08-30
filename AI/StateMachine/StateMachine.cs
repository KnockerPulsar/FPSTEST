using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Copied fron Jason Weimann, https://www.youtube.com/watch?v=V75hgcsCGOM
//Comments by yours truly.

public class StateMachine
{
    private StateInterface currentState;                                        //The current state the entity is in.                                  
    private Dictionary<Type, List<Transition>> transitions =                    //A list of transitions sorted with the state type as the key.
                                new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();       //The list of the possible tranistions from the current state.
    private List<Transition> anyTransitions = new List<Transition>();           //The list of the possible transitions from any state.
    private static List<Transition> emptyTransitions = new List<Transition>(0); //A list of empty transitions that do nothing.

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        currentState.Tick();
    }

    //Checks if the condition of any transitions occured first
    //then continues and checks if the condition of transitions possible form
    //the current state occured, if any condition is met, its state is returned.
    //Otherwise, returns null.
    private Transition GetTransition()
    {
        foreach (var transition in anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in currentTransitions)
            if (transition.Condition())
                return transition;

        return null;

    }

    public void SetState(StateInterface state)
    {
        //Should only continue on state change.
        if (state == currentState)
            return;

        currentState?.OnExit(); //Cleanup for the current state.
        currentState = state;  //Switching states.

        //Checks if the current state has any out transitions
        //and if not, sets the current possible transitions out of the state
        //to an empty transition.
        transitions.TryGetValue(currentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = emptyTransitions;

        currentState.OnEnter(); //Setup for the new state.
    }

    public void AddTransition(StateInterface from, StateInterface to, Func<bool> condition)
    {
        //Checks if the current state has any transitions
        //If not, creates an empty list of transitions to populate.
        if (transitions.TryGetValue(from.GetType(), out var TRNSNS) == false)
        {
            TRNSNS = new List<Transition>();
            transitions[from.GetType()] = TRNSNS;
        }

        TRNSNS.Add(new Transition(to, condition));
    }

    public void AddAnyTransition (StateInterface state, Func<bool> condition)
    {
        anyTransitions.Add(new Transition(state, condition));
    }

    //Just a data container.
    private class Transition
    {
        public Func<bool> Condition { get; }
        public StateInterface To { get; }

        public Transition(StateInterface to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}
