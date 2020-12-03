import React, { Component } from "react";
import ArtistDataService from "../services/artist.service";

export default class AddArtist extends Component {
    constructor(props) {
        super(props);
        this.onChangeName = this.onChangeName.bind(this);
        this.onChangeProfilePictureURL = this.onChangeProfilePictureURL.bind(this);
        this.onChangeBeatportURL = this.onChangeBeatportURL.bind(this);
        this.onChangeSoundCloudURL = this.onChangeSoundCloudURL.bind(this);
        this.saveArtist = this.saveArtist.bind(this);
        this.newArtist = this.newArtist.bind(this);

        this.state = {
            id: null,
            name: "",
            profilePictureURL: "",
            beatportUrl: "",
            soundCloudUrl: ""
        };
    }

    onChangeName(e) {
        this.setState({
            name: e.target.value
        });
    }

    onChangeProfilePictureURL(e) {
        this.setState({
            profilePictureURL: e.target.value
        });
    }

    onChangeBeatportURL(e) {
        this.setState({
            beatportUrl: e.target.value
        });
    }

    onChangeSoundCloudURL(e) {
        this.setState({
            soundCloudUrl: e.target.value
        });
    }

    saveArtist() {
        var data = {
            name: this.state.name,
            profilePictureURL: this.state.profilePictureURL,
            beatportUrl: this.state.beatportUrl,
            soundCloudUrl: this.state.soundCloudUrl 
        };
    
        ArtistDataService.create(data)
        .then(response => {
            this.setState({
            id: response.data.id,
            name: response.data.name,
            profilePictureURL: response.data.profilePictureURL,
            beatportUrl: response.data.beatportUrl,
            soundCloudUrl: response.data.soundCloudUrl,
            published: response.data.published,

            submitted: true
            });
            console.log(response.data);
        })
        .catch(e => {
            console.log(e);
        });
    }

    newArtist() {
        this.setState({
            id: null,
            name: "",
            profilePictureURL: "",
            beatportUrl: "",
            soundCloudUrl: "",
            published: false,

            submitted: false
        })
    }

    render() {
        return (
          <div className="submit-form">
            {this.state.submitted ? (
              <div>
                <h4>You submitted successfully!</h4>
                <button className="btn btn-success" onClick={this.newTutorial}>
                  Add
                </button>
              </div>
            ) : (
              <div>
                <div className="form-group">
                  <label htmlFor="name">Name</label>
                  <input
                    type="text"
                    className="form-control"
                    id="name"
                    required
                    value={this.state.name}
                    onChange={this.onChangeName}
                    name="name"
                  />
                </div>
    
                <div className="form-group">
                  <label htmlFor="profilePictureURL">Profile Picture URL</label>
                  <input
                    type="text"
                    className="form-control"
                    id="description"
                    required
                    value={this.state.profilePictureURL}
                    onChange={this.onChangeProfilePictureURL}
                    name="description"
                  />
                </div>

                

                <div className="form-group">
                  <label htmlFor="beatportUrl">Beatport URL</label>
                  <input
                    type="text"
                    className="form-control"
                    id="beatportUrl"
                    required
                    value={this.state.beatportUrl}
                    onChange={this.onChangeBeatportUrl}
                    name="beatportUrl"
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="soundCloudUrl">SoundCloud URL</label>
                  <input
                    type="text"
                    className="form-control"
                    id="soundCloudUrl"
                    required
                    value={this.state.soundCloudUrll}
                    onChange={this.onChangeSoundCloudUrl}
                    name="soundCloudUrl"
                  />
                </div>
    
                <button onClick={this.saveTutorial} className="btn btn-success">
                  Submit
                </button>
              </div>
            )}
          </div>
        );
    }
}