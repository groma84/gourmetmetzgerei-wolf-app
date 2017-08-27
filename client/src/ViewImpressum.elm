module ViewImpressum exposing (..)

import Html exposing (Html, text, div, h1, h2, h3, span, section, p, ol, li, a, input)
import Html.Attributes exposing (id, href, type_, value, readonly)
import Tachyons exposing (classes)
import Tachyons.Classes exposing (..)
import Update exposing (..)


view : Html Msg
view =
    let
        ispan t =
            span [ classes [ db ] ] [ text t ]
    in
        div [ id "impressum", classes [ pa2, f7 ] ]
            [ Html.h1 [] [ text "Impressum" ]
            , section []
                [ Html.h2 [] [ text "Hinweis: Inoffizielle Webapp" ]
                , p [] [ text "Diese Internetseite ist eine inoffizielle Webseite, die nur dazu dient, eine praktische Anwendung zum Lernen der Programmiersprache Elm zu haben." ]
                , p [] [ text "Sie ist in keinster Weise mit der echten Webseite der Gourmetmetzgerei Wolf verbunden, und auch nicht mit der Gourmet-Metzgerei Wolf!" ]
                , p [] [ text "Alle Angaben sind ohne Gewähr - die Daten auf der Original-Webseite haben immer Vorrang!" ]
                ]
            , section []
                [ Html.h2 [] [ text "Kontaktdaten Gourmet-Metzgerei Wolf" ]
                , ol [ classes [ list ] ]
                    [ li [] [ a [ href "http://www.gourmetmetzgerei-wolf.de/" ] [ text "http://www.gourmetmetzgerei-wolf.de/" ] ]
                    , li []
                        [ input
                            [ type_ "tel"
                            , value "0911 358897"
                            , readonly True
                            , classes [ input_reset, bn ]
                            ]
                            []
                        ]
                    , li [] [ text "Gourmet-Metzgerei Wolf" ]
                    , li [] [ text "Bucher Str. 14" ]
                    , li [] [ text "90408 Nürnberg" ]
                    ]
                ]
            , section []
                [ Html.h2 [] [ text "Impressum inoffizielle Webapp" ]
                , Html.h3 [] [ text "Angaben gemäß § 5 TMG" ]
                , ispan "Martin Grotz"
                , ispan "Erlanger Str. 60A"
                , ispan "91096 Möhrendorf"
                , Html.h3 [] [ text "Kontakt" ]
                , ispan "Telefon: +491735132381"
                , ispan "E-Mail: martin.grotz@gmx.de"
                , Html.h3 [] [ text "Haftung für Inhalte" ]
                , span [] [ text "Als Diensteanbieter sind wir gemäß § 7 Abs.1 TMG für eigene Inhalte auf diesen Seiten nach den allgemeinen Gesetzen verantwortlich. Nach §§ 8 bis 10 TMG sind wir als Diensteanbieter jedoch nicht verpflichtet, übermittelte oder gespeicherte fremde Informationen zu überwachen oder nach Umständen zu forschen, die auf eine rechtswidrige Tätigkeit hinweisen. Verpflichtungen zur Entfernung oder Sperrung der Nutzung von Informationen nach den allgemeinen Gesetzen bleiben hiervon unberührt. Eine diesbezügliche Haftung ist jedoch erst ab dem Zeitpunkt der Kenntnis einer konkreten Rechtsverletzung möglich. Bei Bekanntwerden von entsprechenden Rechtsverletzungen werden wir diese Inhalte umgehend entfernen." ]
                , Html.h3 [] [ text "Haftung für Links" ]
                , span [] [ text "Unser Angebot enthält Links zu externen Webseiten Dritter, auf deren Inhalte wir keinen Einfluss haben. Deshalb können wir für diese fremden Inhalte auch keine Gewähr übernehmen. Für die Inhalte der verlinkten Seiten ist stets der jeweilige Anbieter oder Betreiber der Seiten verantwortlich. Die verlinkten Seiten wurden zum Zeitpunkt der Verlinkung auf mögliche Rechtsverstöße überprüft. Rechtswidrige Inhalte waren zum Zeitpunkt der Verlinkung nicht erkennbar. Eine permanente inhaltliche Kontrolle der verlinkten Seiten ist jedoch ohne konkrete Anhaltspunkte einer Rechtsverletzung nicht zumutbar. Bei Bekanntwerden von Rechtsverletzungen werden wir derartige Links umgehend entfernen." ]
                , Html.h3 [] [ text "Urheberrecht" ]
                , span [] [ text "Die durch die Seitenbetreiber erstellten Inhalte und Werke auf diesen Seiten unterliegen dem deutschen Urheberrecht. Die Vervielfältigung, Bearbeitung, Verbreitung und jede Art der Verwertung außerhalb der Grenzen des Urheberrechtes bedürfen der schriftlichen Zustimmung des jeweiligen Autors bzw. Erstellers. Downloads und Kopien dieser Seite sind nur für den privaten, nicht kommerziellen Gebrauch gestattet. Soweit die Inhalte auf dieser Seite nicht vom Betreiber erstellt wurden, werden die Urheberrechte Dritter beachtet. Insbesondere werden Inhalte Dritter als solche gekennzeichnet. Sollten Sie trotzdem auf eine Urheberrechtsverletzung aufmerksam werden, bitten wir um einen entsprechenden Hinweis. Bei Bekanntwerden von Rechtsverletzungen werden wir derartige Inhalte umgehend entfernen." ]
                ]
            , section []
                [ Html.h2 [] [ text "Datenschutz" ]
                , span [] [ text "Die Betreiber dieser Seiten nehmen den Schutz Ihrer persönlichen Daten sehr ernst. Wir behandeln Ihre\npersonenbezogenen Daten vertraulich und entsprechend der gesetzlichen Datenschutzvorschriften sowie\ndieser Datenschutzerklärung.\nDie Nutzung unserer Webseite ist in der Regel ohne Angabe personenbezogener Daten möglich. Soweit\nauf unseren Seiten personenbezogene Daten (beispielsweise Name, Anschrift oder E-Mail-Adressen)\nerhoben werden, erfolgt dies, soweit möglich, stets auf freiwilliger Basis. Diese Daten werden ohne Ihre\nausdrückliche Zustimmung nicht an Dritte weitergegeben.\nWir weisen darauf hin, dass die Datenübertragung im Internet (z.B. bei der Kommunikation per E-Mail)\nSicherheitslücken aufweisen kann. Ein lückenloser Schutz der Daten vor dem Zugriff durch Dritte ist nicht\nmöglich." ]
                , Html.h2 [] [ text "Server-Log-Files" ]
                , span [] [ text "Der Provider der Seiten erhebt und speichert automatisch Informationen in so genannten Server-Log\nFiles, die Ihr Browser automatisch an uns übermittelt. Dies sind:\nBrowsertyp und Browserversion, \nverwendetes Betriebssystem, \nReferrer URL, \nHostname des zugreifenden Rechners, \nUhrzeit der Serveranfrage.\nDiese Daten sind nicht bestimmten Personen zuordenbar. Eine Zusammenführung dieser Daten mit\nanderen Datenquellen wird nicht vorgenommen. Wir behalten uns vor, diese Daten nachträglich zu \nprüfen, wenn uns konkrete Anhaltspunkte für eine rechtswidrige Nutzung bekannt werden.\n" ]
                , Html.h2 [] [ text "Widerspruch Werbe-Mails" ]
                , span [] [ text "Der Nutzung von im Rahmen der Impressumspflicht veröffentlichten Kontaktdaten zur Übersendung von\nnicht ausdrücklich angeforderter Werbung und Informationsmaterialien wird hiermit widersprochen. Die\nBetreiber der Seiten behalten sich ausdrücklich rechtliche Schritte im Falle der unverlangten Zusendung\nvon Werbeinformationen, etwa durch Spam-E-Mails, vor." ]
                ]
            ]
