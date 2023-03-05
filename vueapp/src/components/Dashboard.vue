<template>
    <v-app-bar color="blue-darken-1" app dark dense>
        <!--<v-app-bar-nav-icon @click="drawer = true" ></v-app-bar-nav-icon>-->
        <v-toolbar-title>ПС для конвертации валют</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn icon raised>
            <v-icon>mdi-magnify</v-icon>
        </v-btn>
        <v-menu left
                bottom>
            <template v-slot:activator="{ props }">
                <v-btn text v-bind="props"> 
                    {{txtUsername}}
                </v-btn>
            </template>
            <v-list nav dense>
                <v-list-item  v-bind:key="1"  @click="showAccount()">
                    <v-list-item-title>Личный кабинет</v-list-item-title>
                </v-list-item>
                <v-list-item v-bind:key="2" @click="logout()">
                    <v-list-item-title>Выйти</v-list-item-title>
                </v-list-item>
            </v-list>
        </v-menu>
    </v-app-bar>
    <template>
        <v-toolbar color="blue-grey-darken-3 mt-5">
            <v-spacer></v-spacer>
            <div class="text-h6" v-for="item in currencies" :key="item.id">
                {{item.cur_Scale}}
                {{item.cur_Abbreviation}}
                {{item.cur_OfficialRate}}
            </div>
            <v-spacer></v-spacer>
        </v-toolbar>
    </template>
    <v-main style="background-color: #f5f6f7">
        <!--<v-main style="background-color: #f5f6f7">-->
        <template>
            <v-row justify="center">
                <v-dialog v-model="dialogAccount"
                          fullscreen
                          :scrim="false"
                          transition="dialog-bottom-transition">
                    <v-card>
                        <v-toolbar dark
                                   color="blue-lighten-4">
                            <v-btn icon
                                   dark
                                   @click="dialogAccount = false">
                                <v-icon>mdi-close</v-icon>
                            </v-btn>
                            <v-toolbar-title>Личный кабинет</v-toolbar-title>
                            <v-spacer></v-spacer>
                        </v-toolbar>
                        <v-row no-gutters>
                            <v-col cols="4">
                                <v-sheet class="pa-2 ma-2">

                                </v-sheet>
                            </v-col>
                            <v-col cols="8">
                                <v-card class="mt-5">
                                    <v-card-title>
                                        <span class="text-h5">Личные данные</span>
                                    </v-card-title>
                                    <v-card-text>
                                        <v-form ref="userForm">
                                            <v-row>
                                                <v-col cols="12" padding="6px"
                                                       sm="6"
                                                       md="6">
                                                    <v-text-field clearable v-model="user.firstName"
                                                                  label="Имя"
                                                                  :rules="[v => !!v || 'Поле должно быть заполено']"
                                                                  required></v-text-field>
                                                    <v-text-field clearable v-model="user.lastName"
                                                                  :rules="[v => !!v || 'Поле должно быть заполено']"
                                                                  required
                                                                  label="Фамилия"></v-text-field>
                                                </v-col>
                                                <v-col cols="12" padding="6px"
                                                       sm="6"
                                                       md="6">
                                                    <v-text-field clearable v-model="user.company"
                                                                  :rules="[v => !!v || 'Поле должно быть заполено']"
                                                                  required
                                                                  label="Организация"></v-text-field>
                                                    <v-text-field clearable v-model="user.location"
                                                                  :rules="[v => !!v || 'Поле должно быть заполено']"
                                                                  required
                                                                  label="Адрес организации"></v-text-field>
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" padding="6px"
                                                       sm="6"
                                                       md="6">
                                                    <v-text-field clearable v-model="user.email"
                                                                  :rules="[ v => /.+@.+/.test(v) || 'Некорректный адрес электронной почты']"
                                                                  required
                                                                  label="Электронная почта"></v-text-field>
                                                </v-col>
                                                <v-col cols="12" padding="6px"
                                                       sm="6"
                                                       md="6">
                                                    <v-text-field clearable v-model="user.phone"
                                                                  :rules="[v => !!v || 'Поле должно быть заполено']"
                                                                  required
                                                                  label="Номер телефона"></v-text-field>
                                                   
                                                </v-col>
                                            </v-row>
                                            <v-row>
                                                <v-col cols="12" padding="6px"
                                                       sm="12"
                                                       md="12">
                                                    <v-textarea outlined
                                                                rows="4" v-model="user.bio"
                                                                name="Discription"
                                                                label="О себе"></v-textarea>
                                                </v-col>
                                            </v-row>
                                        </v-form>
                                    </v-card-text>
                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="blue-darken-1"
                                               variant="text"
                                               @click="save()">
                                            Обновить данные
                                        </v-btn>
                                    </v-card-actions>

                                </v-card>
                            </v-col>
                        </v-row>
                    </v-card>
                </v-dialog>
            </v-row>
        </template>
        <!--<Layout title="Dashboard"></Layout>-->
        <v-container>
            <v-row no-gutters>
                <v-col sm="12"
                       lg="8"
                       col="12">
                    <v-card class="pa-2 w-80" :flat="true" :elevation="1" :height="400" padding="10px">
                        <template v-slot:title>
                            Конвертация валюты по курсу НБ РБ на {{timeCurrencyActual}}
                        </template>
                        <v-card-text>
                            <v-form ref="currencyForm">
                                <v-container fluid>
                                    <v-row>
                                        <v-col cols="4">
                                            <v-select v-model="curFirst"
                                                      :items="currencies"
                                                      item-value="cur_Name"
                                                      item-title="cur_Name"
                                                      filled
                                                      :rules="[(v) => !!v || 'Выберите валюту']"
                                                      label="Выберите валюту"
                                                      @update:modelValue="changeFrom(false)"
                                                      :return-object="true"
                                                      required></v-select>

                                        </v-col>
                                        <v-col cols="8">
                                            <v-text-field label="Введите значение" @input="changeFrom(true)" v-model="curValueFirst" type="number" :rules="[(v) => v>=0 && !!v || 'Число должно быть больше 0']" required></v-text-field>
                                        </v-col>
                                    </v-row>
                                    <v-row>
                                        <v-col cols="4">
                                            <v-select v-model="cur_secound"
                                                      :items="currencies"
                                                      auto-select-first
                                                      item-value="cur_Name"
                                                      item-title="cur_Name"
                                                      filled
                                                      :rules="[(v) => !!v || 'Выберите валюту']"
                                                      label="Выберите валюту"
                                                      @update:modelValue="changeFrom(true)"
                                                      required
                                                      :return-object="true"></v-select>

                                        </v-col>
                                        <v-col cols="8">
                                            <v-text-field label="Введите значение" type="number" :rules="[(v) => v>=0 && !!v || 'Число должно быть больше 0']" @input="changeFrom(false)" v-model="curValueSecound" required></v-text-field>
                                        </v-col>
                                    </v-row>
                                </v-container>
                                <v-btn @click="saveCurrency()" color="success" block class="mt-2">Сохранить конвертацию</v-btn>
                            </v-form>
                        </v-card-text>
                    </v-card>
                </v-col>
                <v-col sm="12"
                       lg="4"
                       col="12">
                    <v-card class="pa-5" :height="400" padding="10px">
                        <v-card-title>
                            <span class="text-h5">Выполненные конвертации</span>
                        </v-card-title>
                        <v-list-item v-for="currency in currenciesChanges"
                                     :key="currency.id">
                            <v-list-item-title>{{formatDateWithHour(currency.timeStamp)}}</v-list-item-title>
                            <v-list-item-subtitle>{{currency.firstTotal}} {{currency.firstCurrency}} | {{currency.secoundTotal}} {{currency.secoundCurrency}}</v-list-item-subtitle>
                        </v-list-item>
                        <v-card-actions>
                            <v-spacer></v-spacer>
                        </v-card-actions>
                    </v-card>
                </v-col>
            </v-row>
        </v-container>
    </v-main>

</template>

<script>
    //import Layout from './Layout.vue';
    import axios from 'axios';
    import Swal from 'sweetalert2';
    import moment from 'moment';
    export default ({
        data() {
            return {
                students: [],
                dialogAccount: false,
                user: [],
                currencies: [],
                currenciesChanges: [],
                curFirst: '',
                cur_secound: '',
                curValueFirst: '',
                curValueSecound: '',
                emailRules: [
                    v => !v || /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) || 'E-mail must be valid'
                ]
            }
        },
        /*components: {
            Layout
        },
        */
        setup() {

        },
        computed: {
            txtUsername() {
                var user = localStorage.getItem('user');
                //console.log(JSON.parse(user)?.userId);
                var userId = JSON.parse(user).email;
                if (userId !== undefined)
                    return userId;
                else return '';
            },
            timeCurrencyActual() {
                var time = JSON.parse(JSON.stringify(this.currencies))[0]?.updateDataInDb;
                if (time !== undefined)
                    return moment(time).format('DD.MM.YYYY HH:MM');
                else return '';
            }
        },
        methods: {
            logout() {
                window.localStorage.clear();
                this.$router.push({ name: 'Login' });
            },
            getTokenConfig() {
                var token = JSON.parse(localStorage.getItem('token'));
                const config = {
                    headers: { Authorization: `Bearer ${token}` }
                };
                return config;
            },
            changeFrom(tt) {
                var curFirst = JSON.parse(JSON.stringify(this.curFirst));
                var cur_secound = JSON.parse(JSON.stringify(this.cur_secound));
                if (curFirst !== undefined && cur_secound !== undefined) {
                    if (this.curValueFirst >= 0 && this.curValueSecound >= 0) {
                        var value1 = curFirst.cur_OfficialRate / curFirst.cur_Scale;
                        var value2 = cur_secound.cur_OfficialRate / cur_secound.cur_Scale;
                        //console.log(value1);
                        //console.log(value2);
                        if (value1 && value2) {
                            if (tt) {
                                this.curValueSecound = ((value1 / value2) * this.curValueFirst).toFixed(5);
                            } else {
                                this.curValueFirst = ((value2 / value1) * this.curValueSecound).toFixed(5);
                            }
                        }
                    }
                }

            },
            async showAccount() {
                this.dialogAccount = true;
                await this.getUser();
            },
            getCurrencies() {
                axios.get("/api/currency/GetCurrencies/", this.getTokenConfig())
                    .then(response => {
                        this.currencies = response.data;
                        console.log(this.currencies);
                    })
                    .catch(error => {
                        if (error.response) {
                            Swal.fire(error.response.data);
                        }
                    });
            },
            getCurrenciesChanges() {
                var user = localStorage.getItem('user');
                var userId = JSON.parse(user)?.userId;
                axios.get("/api/currency/GetCurrencyChanges/" + userId, this.getTokenConfig())
                    .then(response => {
                        this.currenciesChanges = response.data;
                        console.log(this.currencies);
                    })
                    .catch(error => {
                        if (error.response) {
                            Swal.fire(error.response.data);
                        }
                    });
            },
            async getUser() {
                var user = localStorage.getItem('user');
                var userId = JSON.parse(user)?.userId;
                await axios.get("/api/user/GetUser/" + userId, this.getTokenConfig())
                    .then(response => {
                        this.user = response.data;
                        console.log(this.user);
                    })
                    .catch(error => {
                        if (error.response) {
                            Swal.fire(error.response.data);
                        }
                    });
            },
            async saveCurrency() {
                const { valid } = await this.$refs.currencyForm.validate()
                if (valid) {
                    var curFirst = JSON.parse(JSON.stringify(this.curFirst));
                    var cur_secound = JSON.parse(JSON.stringify(this.cur_secound));
                    var user = localStorage.getItem('user');
                    //console.log(JSON.parse(user)?.userId);
                    var userId = JSON.parse(user)?.userId;
                    if (curFirst.cur_Abbreviation !== undefined && cur_secound.cur_Abbreviation !== undefined) {
                        if (this.curValueFirst >= 0 && this.curValueSecound >= 0) {
                            var data = {
                                Id: 0,
                                UserId: userId,
                                FirstCurrency: curFirst.cur_Abbreviation,
                                FirstTotal: Number(this.curValueFirst).toFixed(3).toString().replace(".", ","),
                                SecoundCurrency: cur_secound.cur_Abbreviation,
                                SecoundTotal: Number(this.curValueSecound).toFixed(3).toString().replace(".", ","),
                            };
                            //console.log(data);

                            var formData = new FormData();
                            for (var key in data) {
                                formData.append(key, data[key]);
                               // console.log(data[key]);
                            }
                            axios.post("/api/Currency/SaveCurrencyChange", formData, this.getTokenConfig())
                                .then(response => {
                                    this.currenciesChanges = response.data;
                                    console.log(response.data);
                                    Swal.fire("Конвертация сохранена");
                                })
                                .catch(error => {
                                    if (error.response) {
                                        Swal.fire(error.response.data);
                                    }
                                });
                        }
                    }
                }
            },
            async save() {
                const { valid } = await this.$refs.userForm.validate();
                if (valid) {
                    if (this.user !== undefined) {
                        this.user.password = "hook";
                        this.user.token = "";
                        this.user.message = "";
                        //var formData = new FormData();
                        //for (var key in this.user) {
                        //    formData.append(key, this.user[key]);
                        //}
                        await axios.post("/api/user/SaveUser", this.user, this.getTokenConfig())
                            .then(response => {
                                if (response.data.userId !== '00000000-0000-0000-0000-000000000000') {
                                    this.user = response.data;
                                    alert("Данные обнавлены");
                                } else {
                                    Swal.fire("Ошибка : Что-то случилось не так :(");
                                }
                            })
                            .catch(error => {
                                if (error.response) {
                                    Swal.fire(error.response.data);
                                }
                            });
                    }
                }
                // }
            },
            formatDateWithHour: function (date) {
                return moment(date).format('DD.MM.YYYY HH:MM');
            },
        },
        mounted: function () {
            this.$nextTick(this.getCurrencies())
            this.$nextTick(this.getCurrenciesChanges())
        }
    })
</script>

<style scoped>
</style>
