import React, { Component } from "react";
import ArtistDataService from "../services/artist.service";
import { Link } from "react-router-dom";

export default class ArtistList extends Component {
    constructor(props) {
      super(props);
      this.onChangeSearchName = this.onChangeSearchName.bind(this);
      this.retrieveArtists = this.retrieveArtists.bind(this);
      this.refreshList = this.refreshList.bind(this);
      this.setActiveArtist = this.setActiveTutorial.bind(this);
      this.removeAllArtists = this.removeAllArtists.bind(this);
      this.searchName = this.searchName.bind(this);
  
      this.state = {
        artists: [],
        currentArtist: null,
        currentIndex: -1,
        searchName: ""
      };
    }

    componentDidMount() {
        this.retrieveArtists();
    }

    onChangeSearchArtist(e) {
        const searchName = e.target.value;
    
        this.setState({
          searchName: searchName
        });
      }