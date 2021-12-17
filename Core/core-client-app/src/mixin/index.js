import axios from 'axios'


export default {

    methods: {
        getJwtToken() {
            var urlJwt = this.$store.state.envUrl + '/api/jwt'
            axios.post(urlJwt, {
                Name: 'Nior',
                Email: 'peeran@rvp.co.th'
            }).then((response) => {
                this.$store.state.jwtToken = response.data
            }).catch(function (error) {
                alert(error)
            })
        },
    }
}
